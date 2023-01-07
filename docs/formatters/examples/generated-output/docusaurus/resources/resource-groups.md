---


tags:

- Microsoft.Resources

---


# Resource Group

> A module to demonstrate documentation generation

## Usage

```bicep
module exampleInstance 'ts/IacModules:resources/resource-groups:2022-12-17' = {
  name: 'exampleInstance'
  params: {
    boolInput: false
    complexObject: {
      prop: 'one'
      propTwo: 'two'
    }
    inputArray: [
      { prop: 'one' }
    ]
    inputArraySimple: [
      'one'
      'two'
    ]
    intInput: 124
    resourceGroupLocation: resourceGroupLocation
    resourceGroupName: 'resourceGroupName'
    tags: {}
  }
}
```

## Parameters

| Parameter | Description | Type | Default |
| --- | --- | --- | --- |
| `boolInput` | Bool input | bool | false |
| `complexObject` | Object input | object | [complexObjectValue](#complexobjectvalue) |
| `inputArray` | Complex array input | array | [inputArrayValue](#inputarrayvalue) |
| `inputArraySimple` | Simple array input | array | [  'one'  'two'] |
| `intInput` | int input | int | 124 |
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
  prop: 'one'
  propTwo: 'two'
}
```

### inputArrayValue

```bicep
[
  { prop: 'one' }
]
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