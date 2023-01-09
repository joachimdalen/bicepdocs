# Bicep Documentation Generator

**BicepDocs is currently under development. Please report issues you find. Also see the list of known issues before reporting.**

> Bicepdocs is not endorsed or affiliated with Microsoft

BicepDocs is a tool to build documentation for [Bicep](https://github.com/Azure/bicep) modules. It aims to provide a way to easily document your custom modules without having to manually maintain and update documentation.

See a quick example for the [bicep file](./docs/formatters/examples/inputs/resources/resource-groups.bicep) that has been converted to a [markdown file](./docs/formatters/examples/generated-output/resources/resource-groups.md) using the [markdown provider](./docs/formatters/markdown.md).

Check the [Roadmap](https://github.com/joachimdalen/bicepdocs/issues) for planned features and changes.

## Quickstart

To generate documentation from modules on the file system

```sh
bicepdocs generate filesystem \
--folderPath "path-to-input-folder" \
--out "path-to-write-files-to" \
--formatter markdown \
[--config "path-to-config-file"]

```

## Installation

### macOS

**Using Homebrew:**

```bash
# Add the bicepdocs tap
brew tap joachimdalen/bicepdocs

# Install bicepdocs
brew install bicepdocs
```

**Using BASH:**

```bash
# Get the latest released binary
curl -Lo bicepdocs https://github.com/joachimdalen/bicepdocs/releases/latest/download/bicepdocs-osx-x64

# Make the file executable
chmod +x ./bicepdocs

# Add Gatekeeper exception (requires admin)
sudo spctl --add ./bicepdocs

# Add the executable to your PATH
sudo mv ./bicepdocs /usr/local/bin/bicepdocs

# Verify the installation
bicepdocs --help
```

### Linux

```pwsh
# Get the latest released binary
curl -Lo bicepdocs https://github.com/joachimdalen/bicepdocs/releases/latest/download/bicepdocs-linux-x64

# Make the file executable
chmod +x ./bicepdocs

# Add the executable to your PATH
sudo mv ./bicepdocs /usr/local/bin/bicepdocs

# Verify the installation
bicepdocs --help
```

### Windows

Other installation methods are planned: [Improve installation experience for Windows users](https://github.com/joachimdalen/bicepdocs/issues/19)

**Using Powershell:**

```pwsh
# Create the install folder
$installPath = "$env:USERPROFILE\.bicepdocs"
$installDir = New-Item -ItemType Directory -Path $installPath -Force
$installDir.Attributes += 'Hidden'

# Get the latest released binary
(New-Object Net.WebClient).DownloadFile("https://github.com/joachimdalen/bicepdocs/releases/latest/download/bicepdocs-win-x64.exe", "$installPath\bicepdocs.exe")

# Add the executable to your PATH
$currentPath = (Get-Item -path "HKCU:\Environment" ).GetValue('Path', '', 'DoNotExpandEnvironmentNames')
if (-not $currentPath.Contains("%USERPROFILE%\.bicepdocs")) { setx PATH ($currentPath + ";%USERPROFILE%\.bicepdocs") }
if (-not $env:path.Contains($installPath)) { $env:path += ";$installPath" }

# Verify the installation
bicepdocs --help
```

## Supported Sources

The source is responsible for fetching the bicep files and providing the [Formatter](#supported-formatters) with the text representation of the file.

| Source     | Description                            |
| ---------- | -------------------------------------- |
| filesystem | Loads bicep files from the file system |

## Supported Formatters

The formatter is responsible for converting the bicep file into the wanted documentation format. Currently all formatters is based on markdown.

| Formatter                                     | Description                                                                                      |
| --------------------------------------------- | ------------------------------------------------------------------------------------------------ |
| [markdown](./docs/formatters/markdown.md)     | Converts bicep files and formats them as markdown                                                |
| [docusaurus](./docs/formatters/docusaurus.md) | Converts bicep files and formats them as markdown while adding metadata and files for Docusaurus |

## Supported Destinations

The destination takes the formatted documentation file and writes it to the wanted location.

| Destination | Description                                   |
| ----------- | --------------------------------------------- |
| filesystem  | Writes the formatted files to the file system |

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

| Option                     | Description                                                                                         |
| -------------------------- | --------------------------------------------------------------------------------------------------- |
| `includeExistingResources` | When `true` will include referenced resources using the `existing` keyword in the resources list    |
| `metaKeyword`              | Metadata definition keyword used for module metadata                                                |
| `includeParameters`        | Include the parameters section                                                                      |
| `includeUsage`             | Include the code example /usage section                                                             |
| `includeResources`         | Include the list of resources                                                                       |
| `includeOutputs`           | Inlude the outputs section                                                                          |
| `sectionOrder`             | Section order for the generated document                                                            |
| `disableVersioning`        | Do not generate versioned output folders                                                            |
| `formatters`               | Options for the individual formatters. See the documentation for the formatter for more information |
