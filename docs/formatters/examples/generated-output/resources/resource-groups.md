# Resource Group

> A module to demonstrate documentation generation

## Usage

```bicep
module exampleInstance 'br/MyRegistry:bicep/modules/customModule:2022-10-29' = {
  name: 'exampleInstance'
  params: {
    boolInput: False
    complexObject: {
  propTwo: 'two'
  prop: 'one'
}

    inputArray: [{
  prop: 'one'
}]
    inputArraySimple: ['one' 'two' ]
    intInput: intInput
    resourceGroupLocation: resourceGroupLocation
    resourceGroupName: 'resourceGroupName'
    tags: {}
}

```

## Parameters

| Parameter | Description | Type | Default |
| --- | --- | --- | --- |
| `boolInput` | Bool input | bool | False |
| `complexObject` | Object input | object | [complexObjectValue](#complexobjectvalue) |
| `inputArray` | Complex array input | array | [inputArrayValue](#inputarrayvalue) |
| `inputArraySimple` | Simple array input | array | ['one' 'two' ] |
| `intInput` | int input | int |  |
| `resourceGroupLocation` | Location of the resource group | [resourceGroupLocationAllow](#resourcegrouplocationallow) |  |
| `resourceGroupName` | Name of the resource group | string |  |
| `tags` | Tags to append to resource group | object | {} |

## Resources

- [Microsoft.Resources/resourceGroups@2021-01-01](https://learn.microsoft.com/en-us/azure/templates/microsoft.resources/2021-01-01/resourcegroups)

## Outputs

| Name | Type | Description |
| --- | --- | --- |
| `resourceId` | string | ResourceId of the resource group |
| `location` | string | Location of the resource group |

## References

### complexObjectValue

```bicep
{
  propTwo: 'two'
  prop: 'one'
}

```

### inputArrayValue

```bicep
[{
  prop: 'one'
}]
```

### resourceGroupLocationAllow

- northcentralus
- northcentralusstage
- northeurope
- norway
- norwayeast
- norwaywest
- uk
- uksouth
- ukwest
- westeurope