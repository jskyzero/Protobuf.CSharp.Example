#!/bin/bash
for filename in src/proto/*.proto; do
  # build proto (make sure you hava add protoc to your path)
  protoc --proto_path=src/proto \
        --csharp_out=src/proto.cs \
        $filename
done