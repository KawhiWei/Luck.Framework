#!/bin/bash
echo "请输入NUGET_API_KEY"
read NUGET_API_KEY
dotnet build -c Release
dotnet pack -c Release
#dotnet nuget push bin/Release/*.nupkg -k $NUGET_API_KEY -s https://api.nuget.org/v3/index.json