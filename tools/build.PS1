# this script use to build proto
# (windows only)

$files = Get-ChildItem src/proto
foreach ($file in $files) {
  # build proto
./tools/protoc.exe `
  --proto_path=src/proto `
  --csharp_out=src/proto.cs `
  $file
}