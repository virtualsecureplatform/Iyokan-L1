# Iyokan-L1

# Build
## Dependency
please install below package
```
dotnet
```

## Build
Build Iyokan-L1 and convert yosys result json to original format.
```
dotnet run -i vsp-core.json -o vsp-core-converted.json
```
```
dotnet run -i <intputFileName> -o <outputFileName>
```

if you want to integrate circuit with rom, add `--with-rom` option
```
dotnet run -i vsp-core.json -o vsp-core-converted.json --with-rom
```
```
dotnet run -i <intputFileName> -o <outputFileName> --with-rom
```

```
-i, --input     Required. Input yosys format json file
-o, --output    Required. Output iyokan format json file
--with-rom      Integrate ROM
```

# Changelog
- v0.0.1 2019/12/04 - PoC Implementation
