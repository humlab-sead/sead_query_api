#!/bin/bash

echo "inside entrypoint"

dotnet sead.query.api.dll /var/log/query.sead.api.log 2>&1
