Remove-Item ./autogenerated/* -Recurse
docker run --rm -it -v ${pwd}:/root -v ${pwd}/autogenerated:/out `
openapitools/openapi-generator-cli generate -o /out -i http://goal-api.azurewebsites.net/swagger/v1/swagger.json -g typescript-fetch