#!/usr/bin/env bash

set -euo pipefail

base_url="${SEAD_QUERY_API_BASE_URL:-${1:-http://localhost:8090}}"
base_url="${base_url%/}/"
samples="${SEAD_QUERY_API_MEASURE_SAMPLES:-3}"
warmups="${SEAD_QUERY_API_MEASURE_WARMUPS:-1}"
warn_seconds="${SEAD_QUERY_API_MEASURE_WARN_SECONDS:-}"

tmp_dir="$(mktemp -d)"
trap 'rm -rf "$tmp_dir"' EXIT

fail() {
    echo "error: $*" >&2
    exit 1
}

require_positive_integer() {
    local value="$1"
    local label="$2"

    if ! [[ "$value" =~ ^[1-9][0-9]*$ ]]; then
        fail "${label} must be a positive integer, got '${value}'"
    fi
}

require_optional_number() {
    local value="$1"
    local label="$2"

    if [[ -n "$value" ]] && ! [[ "$value" =~ ^[0-9]+([.][0-9]+)?$ ]]; then
        fail "${label} must be empty or numeric, got '${value}'"
    fi
}

require_in_file() {
    local file_path="$1"
    local expected="$2"
    local label="$3"

    if ! grep -Fq "$expected" "$file_path"; then
        echo "error: ${label} response did not contain expected text: ${expected}" >&2
        echo "error: response body follows:" >&2
        cat "$file_path" >&2
        exit 1
    fi
}

require_regex_in_file() {
    local file_path="$1"
    local expected_regex="$2"
    local label="$3"

    if ! grep -Eq "$expected_regex" "$file_path"; then
        echo "error: ${label} response did not match expected pattern: ${expected_regex}" >&2
        echo "error: response body follows:" >&2
        cat "$file_path" >&2
        exit 1
    fi
}

write_payload() {
    local payload_file="$1"
    local payload_json="$2"

    printf '%s\n' "$payload_json" > "$payload_file"
}

perform_request() {
    local method="$1"
    local path="$2"
    local payload_file="$3"
    local output_file="$4"

    if [[ "$method" == "GET" ]]; then
        curl --fail --silent --show-error \
            --location \
            --output "$output_file" \
          --write-out '%{http_code} %{time_total}\n' \
            "${base_url}${path}"
        return
    fi

    curl --fail --silent --show-error \
        --location \
        --header 'Content-Type: application/json' \
        --data @"$payload_file" \
        --output "$output_file" \
      --write-out '%{http_code} %{time_total}\n' \
        "${base_url}${path}"
}

validate_response() {
    local output_file="$1"
    local response_regex="$2"
    local label="$3"
    shift 3

    require_regex_in_file "$output_file" "$response_regex" "$label"

    for expected in "$@"; do
        require_in_file "$output_file" "$expected" "$label"
    done
}

print_summary() {
    local name="$1"
    local timings_file="$2"
    local summary

    summary="$({
        awk '
            BEGIN { min = -1; max = 0; sum = 0; count = 0 }
            {
                value = $1 + 0;
                if (min < 0 || value < min) min = value;
                if (value > max) max = value;
                sum += value;
                count += 1;
            }
            END {
                if (count == 0) {
                    exit 1;
                }

                printf "count=%d avg=%.3fs min=%.3fs max=%.3fs", count, sum / count, min, max;
            }
        ' "$timings_file"
    })"

    echo "measure: ${name} ${summary}"

    if [[ -n "$warn_seconds" ]] && awk -v file="$timings_file" -v threshold="$warn_seconds" '
        BEGIN { exceeded = 0 }
        {
            value = $1 + 0;
            if (value > threshold) {
                exceeded = 1;
            }
        }
        END { exit exceeded ? 0 : 1 }
    ' "$timings_file"; then
        echo "warn: ${name} exceeded SEAD_QUERY_API_MEASURE_WARN_SECONDS=${warn_seconds}s on at least one measured sample"
    fi
}

measure_request() {
    local name="$1"
    local method="$2"
    local path="$3"
    local payload_json="$4"
    local response_regex="$5"
    shift 5

    local output_file="$tmp_dir/${name}.response.json"
    local payload_file="$tmp_dir/${name}.payload.json"
    local timings_file="$tmp_dir/${name}.timings"

    : > "$timings_file"

    if [[ -n "$payload_json" ]]; then
        write_payload "$payload_file" "$payload_json"
    else
        : > "$payload_file"
    fi

    echo "info: warming ${name} via ${base_url}${path}"

    for warmup in $(seq 1 "$warmups"); do
        local warmup_http_code
        local warmup_time_total

        read -r warmup_http_code warmup_time_total < <(perform_request "$method" "$path" "$payload_file" "$output_file")
        [[ "$warmup_http_code" == "200" ]] || fail "${name} warm-up returned HTTP ${warmup_http_code}"
        validate_response "$output_file" "$response_regex" "$name warm-up" "$@"
    done

    echo "info: measuring ${name} with ${samples} sample(s)"

    for sample in $(seq 1 "$samples"); do
        local http_code
        local time_total

        read -r http_code time_total < <(perform_request "$method" "$path" "$payload_file" "$output_file")
        [[ "$http_code" == "200" ]] || fail "${name} sample ${sample} returned HTTP ${http_code}"
        validate_response "$output_file" "$response_regex" "$name sample ${sample}" "$@"
        printf '%s\n' "$time_total" >> "$timings_file"
        echo "info: ${name} sample ${sample}/${samples} time_total=${time_total}s"
    done

    print_summary "$name" "$timings_file"
}

require_positive_integer "$samples" "SEAD_QUERY_API_MEASURE_SAMPLES"
require_positive_integer "$warmups" "SEAD_QUERY_API_MEASURE_WARMUPS"
require_optional_number "$warn_seconds" "SEAD_QUERY_API_MEASURE_WARN_SECONDS"

echo "info: warming service via ${base_url}api/version"
measure_request \
    "version" \
    "GET" \
    "api/version" \
    "" \
    '^[[:space:]]*\[[[:space:]]*"[0-9]+'

measure_request \
    "country-map-result" \
    "POST" \
    "api/result/load" \
    '{
  "facetsConfig": {
    "RequestId": "1",
    "DomainCode": "",
    "RequestType": "populate",
    "TargetCode": "country",
    "FacetConfigs": [
      {
        "FacetCode": "country",
        "Position": 0,
        "TextFilter": "",
        "Picks": [
          {
            "PickValue": "57",
            "Text": "57"
          }
        ]
      }
    ]
  },
  "resultConfig": {
    "RequestId": "1",
    "SessionId": "1",
    "FacetCode": "map_result",
    "ViewTypeId": "map",
    "AggregateKeys": ["site_level"]
  }
}' \
    '"[Qq]uery"' \
    'with composed_filter as' \
    'target_route as' \
    'join target_route on target_route.target_id = tbl_sites.site_id' \
    'join composed_filter on composed_filter.target_id = target_route.source_id'

measure_request \
    "sites-polygon-map-result" \
    "POST" \
    "api/result/load" \
    '{
  "facetsConfig": {
    "RequestId": "1",
    "DomainCode": "",
    "RequestType": "populate",
    "TargetCode": "sites_polygon",
    "FacetConfigs": [
      {
        "FacetCode": "sites_polygon",
        "Position": 0,
        "TextFilter": "",
        "Picks": [
          {
            "PickValue": "63.872484,20.093291,63.947006,20.501316,63.878949,20.673213,63.748021,20.252953,63.793983,20.095738",
            "Text": "63.872484,20.093291,63.947006,20.501316,63.878949,20.673213,63.748021,20.252953,63.793983,20.095738"
          }
        ]
      }
    ]
  },
  "resultConfig": {
    "RequestId": "1",
    "SessionId": "1",
    "FacetCode": "map_result",
    "ViewTypeId": "map",
    "AggregateKeys": ["site_level"]
  }
}' \
    '"[Qq]uery"' \
    'with composed_filter as' \
    'target_route as' \
    'join target_route on target_route.target_id = tbl_sites.site_id' \
    'join composed_filter on composed_filter.target_id = target_route.source_id' \
    'ST_Within('

measure_request \
    "analysis-entity-ages-map-result" \
    "POST" \
    "api/result/load" \
    '{
  "facetsConfig": {
    "RequestId": "1",
    "DomainCode": "",
    "RequestType": "populate",
    "TargetCode": "analysis_entity_ages",
    "FacetConfigs": [
      {
        "FacetCode": "analysis_entity_ages",
        "Position": 0,
        "TextFilter": "",
        "Picks": [
          {
            "PickValue": "850000",
            "Text": "850000"
          },
          {
            "PickValue": "2350000",
            "Text": "2350000"
          }
        ]
      },
      {
        "FacetCode": "sites",
        "Position": 1,
        "TextFilter": "",
        "Picks": []
      }
    ]
  },
  "resultConfig": {
    "RequestId": "1",
    "SessionId": "1",
    "FacetCode": "map_result",
    "ViewTypeId": "map",
    "AggregateKeys": ["site_level"]
  }
}' \
    '"[Qq]uery"' \
    'with composed_filter as' \
    'target_route as' \
    'join target_route on target_route.target_id = tbl_sites.site_id' \
    'join composed_filter on composed_filter.target_id = target_route.source_id' \
    "int4range(850000, 2350000, '[]')"

measure_request \
    "sites-map-result" \
    "POST" \
    "api/result/load" \
    '{
  "facetsConfig": {
    "RequestId": "1",
    "DomainCode": "",
    "RequestType": "populate",
    "TargetCode": "sites",
    "FacetConfigs": [
      {
        "FacetCode": "sites",
        "Position": 0,
        "TextFilter": "",
        "Picks": []
      }
    ]
  },
  "resultConfig": {
    "RequestId": "1",
    "SessionId": "1",
    "FacetCode": "map_result",
    "ViewTypeId": "map",
    "AggregateKeys": ["site_level"]
  }
}' \
    '"[Qq]uery"' \
    'with composed_filter as' \
    'target_route as' \
    'join target_route on target_route.target_id = tbl_sites.site_id' \
    'join composed_filter on composed_filter.target_id = target_route.source_id'

measure_request \
    "geochronology-facet-content" \
    "POST" \
    "api/facets/load" \
    '{
  "RequestId": "1",
  "DomainCode": "",
  "RequestType": "populate",
  "TargetCode": "geochronology",
  "FacetConfigs": [
    {
      "FacetCode": "geochronology",
      "Position": 0,
      "TextFilter": "",
      "Picks": []
    }
  ]
}' \
    '"SqlQuery"' \
    'tbl_geochronology'

measure_request \
    "ceramic-sample-groups-map-result" \
    "POST" \
    "api/result/load" \
    '{
  "facetsConfig": {
    "RequestId": "1",
    "DomainCode": "",
    "RequestType": "populate",
    "TargetCode": "sample_groups",
    "FacetConfigs": [
      {
        "FacetCode": "sample_groups",
        "Position": 0,
        "TextFilter": "",
        "Picks": []
      }
    ]
  },
  "resultConfig": {
    "RequestId": "1",
    "SessionId": "1",
    "FacetCode": "map_result",
    "ViewTypeId": "map",
    "AggregateKeys": ["site_level"]
  }
}' \
    '"[Qq]uery"' \
    'with composed_filter as' \
    'from tbl_analysis_entities' \
    'target_route as' \
    'join target_route on target_route.target_id = tbl_sites.site_id' \
    'join composed_filter on composed_filter.target_id = target_route.source_id'

echo "info: deployment-like HTTP timing checks passed for ${base_url}"
echo "info: interpret these warmed timings comparatively on the same environment; investigate sustained regressions or any sample that crosses SEAD_QUERY_API_MEASURE_WARN_SECONDS when that threshold is set"