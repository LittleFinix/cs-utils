
$key = $args[0];
$repo = "https://nuget.littlefinix.net";
$dist_folder = "nuget_dist";

Remove-Item -Path "$dist_folder" -Force -Confirm:$false -Recurse;
dotnet build -c Release
dotnet pack -c Release -o "$dist_folder";

foreach ($file in $(Get-ChildItem "$dist_folder")) {
    dotnet nuget push --skip-duplicate "$file" -s "$repo" -k "$key";
}
