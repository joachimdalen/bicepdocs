targetScope = 'subscription'

metadata moduleDocs = {
  title: 'Resource Group'
  description: 'A module to demonstrate documentation generation'
  version: '2022-12-17'
}

@description('Name of the resource group')
param resourceGroupName string

@allowed([
  'northcentralus'
  'northcentralusstage'
  'northeurope'
  'norway'
  'norwayeast'
  'norwaywest'
  'uk'
  'uksouth'
  'ukwest'
  'westeurope'
])
@description('Location of the resource group')
param resourceGroupLocation string

@description('Tags to append to resource group')
param tags object = {}

@description('Bool input')
param boolInput bool = false

@description('int input')
param intInput int = 124

@description('Complex array input')
param inputArray array = [
  { prop: 'one' }
]

@description('Simple array input')
param inputArraySimple array = [
  'one'
  'two'
]

@description('Object input')
param complexObject object = {
  prop: 'one'
  propTwo: 'two'
}

@description('Resource to deploy')
resource resourceGroup 'Microsoft.Resources/resourceGroups@2021-01-01' = {
  name: resourceGroupName
  location: resourceGroupLocation
  tags: tags
}

@description('ResourceId of the resource group')
output resourceId string = resourceGroup.id

@description('Location of the resource group')
output location string = resourceGroup.location
