# Function to print the collected values for the current table/columns
# PARAM i: Loop counter (local variable)
function flush_values(  i) {
    if (vc > 0) { # Check if value counter (vc) is greater than 0
        print "INSERT INTO " current_table " (" current_columns ") VALUES" >> output_file
        for (i = 1; i <= vc; i++) {
            terminator = (i == vc) ? ";" : ","
            # Print indented value tuple with correct terminator
            print "    " values_list[i] terminator >> output_file
        }
        print "" >> output_file # Add a blank line after the combined statement
    }
    # Reset state for the next batch
    vc = 0
    # Clear the array
    delete values_list
}

BEGIN {
    # Record Separator: Semicolon followed by optional whitespace/newline
    RS = ";[[:space:]]*"
    FS = "\n" # Field separator (less critical here)

    # Initialize state variables
    current_table = ""
    current_columns = ""
    vc = 0 # values counter

    # Argument handling (assuming gawk -f script.awk input output)
    if (ARGC < 3) {
        print "Error: Missing input or output file argument for awk script." > "/dev/stderr"
        exit 1
    }
    # input_file_awk = ARGV[1] # Input file path (available if needed)
    output_file = ARGV[2]    # Output file path
    ARGC = 2                 # Process only ARGV[1] as input

    # Ensure output file is empty (handled by shell wrapper '>')
}

# Main processing block for each record (SQL statement)
NF > 0 {
    # Trim leading/trailing whitespace from the whole record ($0) first
    gsub(/^[[:space:]]+|[[:space:]]+$/, "", $0)
    record = $0 # Work on a copy

    # Regex captures:
    # 1: table name
    # 2: column list
    # 3: content INSIDE the VALUES (...) parentheses
    # Using (.*) which in gawk's match() context on $0 (potentially multiline due to RS)
    # will match across lines within the record. Anchored by start/end patterns.
    if (match(record, /^[[:space:]]*INSERT[[:space:]]+INTO[[:space:]]+([^[:space:]\(]+)[[:space:]]*\(([^)]+)\)[[:space:]]*VALUES[[:space:]]*\((.*)\)[[:space:]]*$/, parts)) {

        table = parts[1]
        columns = parts[2]
        # Directly use the captured content between the parentheses
        values_content = parts[3]

        # *** Crucial Sanity Check/Trim ***
        # Sometimes, depending on implementation details or subtle input variations,
        # the captured group might include surrounding whitespace that was *inside*
        # the parentheses but before/after the actual value content. Let's trim that.
        gsub(/^[[:space:]]+/, "", values_content)
        gsub(/[[:space:]]+$/, "", values_content)

        # Reconstruct the tuple string, adding the parentheses back
        current_values = "(" values_content ")"

        # --- State Handling (same as before) ---
        if (table != current_table || columns != current_columns) {
            flush_values()
            current_table = table
            current_columns = columns
        }

        values_list[++vc] = current_values

    } else {
        # Record does not match the expected INSERT structure
        if (vc > 0) {
            # Flush any pending data before handling the non-matching line
            flush_values()
            # Reset state since the sequence is broken
            current_table = ""
            current_columns = ""
        }
        # Optional: Log or print the non-matching record
        # print "Ignoring non-matching record: " record >> "/dev/stderr"
        # If you want to pass non-INSERT lines through:
        # if (record ~ /[^[:space:]]/) { # If not just whitespace
        #      print record ";" >> output_file # Print original record + add back semicolon
        #      print "" >> output_file
        # }
    }
}

END {
    # End of input file: flush any remaining collected values
    flush_values()
}