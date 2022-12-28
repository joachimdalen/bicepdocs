# Bicep Documentation Generator

BicepDocs is a tool to build documentation for Bicep modules.

> BicepDocs is currently under development. Please report issues you find. Also see the list of known issues before reporting.

See a quick example for the [bicep file](./docs/providers/examples/inputs/resources/resource-groups.bicep) that has been converted to a [markdown file](./docs/providers/examples/generated-output/resources/resource-groups.md) using the [markdown provider](./docs/providers/markdown-provider.md).

Check the [Roadmap](#roadmap) for planned features and changes.

## Quickstart

To generate documentation from modules on the file system

```sh
bicepdocs generate filesystem \
--folderPath "path-to-input-folder" \
--out "path-to-write-files-to" \
--provider markdown
[--config "path-to-config-file"]

```

## Supported Providers

| Provider   | Description                                                |
| ---------- | ---------------------------------------------------------- |
| markdown   | Writes output files compatible with markdown               |
| docusaurus | Writes markdown files with supported docusaurus formatting |

## Metadata

In certain situations you want to append metadata to your module files. This metadata can be provided to the documentation by adding the following `metadata` definition to your module. The keyword can be changed in the configuration file.

```bicep
metadata moduleDocs = {
  title: 'The title of the module'
  description: 'A description describing the module'
  version: '2022-12-16' // The version will also be used to determine the folder structure
  owner: 'Some User <some.user@email.com>'
}
```

## Configuration

Certain options can be configured using a yaml file. For full reference, see the [example-config.yml](./docs/example-config.yml). Provide the file path to the configuration file using the global paramter `--config`

| Option                     | Description                                                                                      |
| -------------------------- | ------------------------------------------------------------------------------------------------ |
| `includeExistingResources` | When `true` will include referenced resources using the `existing` keyword in the resources list |
| `metaKeyword`              | Metadata definition keyword used for module metadata                                             |
| `includeParameters`        | Include the parameters section                                                                   |
| `includeUsage`             | Include the code example /usage section                                                          |
| `includeResources`         | Include the list of resources                                                                    |
| `includeOutputs`           | Inlude the outputs section                                                                       |
| `sectionOrder`             | Section order for the generated document                                                         |
| `disableVersioning`        | Do not generate versioned output folders                                                         |

## Documentation Generated

See the following documentaion for the different providers

- `--provider markdown` : [Markdown Provider](./docs/providers/markdown-provider.md)
- `--provider docusaurus` : [Docusaurus Provider](./docs/providers/docusaurus-provider.md)

## Roadmap

Some of the features that are on the Roadmap for BicepDocs:

- Generate documentation from published modules in a Container Registry
- [Azure DevOps Wiki Provider (Create and upload the documentation to the wiki)](https://github.com/joachimdalen/bicepdocs/issues/2)
- [Confluence Provider](https://github.com/joachimdalen/bicepdocs/issues/1)
