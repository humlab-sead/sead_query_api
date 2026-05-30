#!/usr/bin/env bash

set -euo pipefail

base_url="${SEAD_QUERY_API_BASE_URL:-${1:-http://localhost:8090}}"
base_url="${base_url%/}/"

tmp_dir="$(mktemp -d)"
trap 'rm -rf "$tmp_dir"' EXIT

fail() {
    echo "error: $*" >&2
    exit 1
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

get_json() {
    local path="$1"
    local output_file="$2"

    curl --fail --silent --show-error \
        --location \
        --output "$output_file" \
        "${base_url}${path}"
}

post_json() {
    local path="$1"
    local payload_file="$2"
    local output_file="$3"

    curl --fail --silent --show-error \
        --location \
        --header 'Content-Type: application/json' \
        --data @"$payload_file" \
        --output "$output_file" \
        "${base_url}${path}"
}

write_payload() {
    local payload_file="$1"
    local payload_json="$2"

    printf '%s\n' "$payload_json" > "$payload_file"
}

run_result_check() {
    local name="$1"
    local payload_json="$2"
    shift 2

    local payload_file="$tmp_dir/${name}.payload.json"
    local response_file="$tmp_dir/${name}.response.json"

    write_payload "$payload_file" "$payload_json"

    echo "info: checking ${name} via ${base_url}api/result/load"
    post_json "api/result/load" "$payload_file" "$response_file"

    require_regex_in_file "$response_file" '"[Qq]uery"' "$name"

    for expected in "$@"; do
        require_in_file "$response_file" "$expected" "$name"
    done
}

version_response="$tmp_dir/version.response.json"

echo "info: checking version endpoint via ${base_url}api/version"
get_json "api/version" "$version_response"
require_regex_in_file "$version_response" '^\[[[:space:]]*"[0-9]+' 'version'

run_result_check \
    "country-map-result" \
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
    'with composed_filter as' \
    'target_route as' \
    'join target_route on target_route.target_id = tbl_sites.site_id' \
    'join composed_filter on composed_filter.target_id = target_route.source_id'

run_result_check \
    "sites-polygon-map-result" \
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
    'with composed_filter as' \
    'target_route as' \
    'join target_route on target_route.target_id = tbl_sites.site_id' \
    'join composed_filter on composed_filter.target_id = target_route.source_id' \
    'ST_Within('

run_result_check \
    "analysis-entity-ages-map-result" \
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
    'with composed_filter as' \
    'target_route as' \
    'join target_route on target_route.target_id = tbl_sites.site_id' \
    'join composed_filter on composed_filter.target_id = target_route.source_id' \
    "int4range(850000, 2350000, '[]')"

echo "info: deployment HTTP smoke checks passed for ${base_url}"