#!/usr/bin/env bash

docker run \
 -e "ASPNETCORE_ENVIRONMENT=Development" \
 -e "AWS_ACCESS_KEY_ID=fakevalue" \
 -e "AWS_SECRET_ACCESS_KEY=fakevalue" \
 -e "AWS_DEFAULT_REGION=us-west-1" \
 -it -p 8880:80 --rm core-api