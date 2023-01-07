# Markdown Provider

## Configuration

Options for the markdown formatter can be provided under `formatters.markdown` in the configuration file. For full reference, see the [example-config.yml](../example-config.yml).

```yaml
markdown:
  usage:
    moduleType: local
    moduleAlias: IacModules
```

| Option              | Description                                                                                   |
| ------------------- | --------------------------------------------------------------------------------------------- |
| `usage.moduleType`  | Determins if the usage examples uses `br` or `ts`. Accepted values are `local` and `registry` |
| `usage.moduleAlias` | The alias used. Same as `moduleAliases` in `bicepconfig.json`                                 |

## Generation

The tool assumes a file structure of something like under the path:
`/home/Users/build/`

```
reg-modules
├── key-vault
│   ├── role-assignments
│   |   └── vaults-rbac.bicep
│   └── vaults.bicep
├── web
│   ├── function-app.bicep
│   ├── function-app-dedicated.bicep
│   └── web-app.bicep
└── resource-group.bicep
```

When passing the path `/home/Users/build/reg-modules` as `--folderPath` the output produced would be:

```
reg-modules-out
├── key-vault
│   ├── role-assignments
│   |   └── vaults-rbac.md
│   └── vaults.md
├── web
│   ├── function-app.md
│   ├── function-app-dedicated.md
│   └── web-app.md
└── resource-group.md
```

The path you would pass as the input path is `/home/Users/build/reg-modules`

## Versioning

When providing the `moduleDocs` and setting the version a set of versioned files will be created.

The following example assumes that the version `2022-12-18` is set for:

- `reg-modules/key-vault/vaults.bicep`
- `reg-modules/key-vault/role-assignments/vaults-rbac.bicep`

```
reg-modules-out
├── key-vault
│   ├── versions
│   |   ├── 2022-12-18
|   │   |   ├── role-assingments
|   |   │   |   └── vaults-rbac.md
|   |   |   └── vaults.md
│   ├── role-assignments
│   |   └── vaults-rbac.md
│   └── vaults.md
├── web
│   └── function-app.md
│   └── function-app-dedicated.md
│   └── web-app.md
└── resource-group.md
```

If you are manually versioning your files, you can disable this logic by setting `disableVersion: true` in the configuration file. The versioning assumes that the first sub directory under the `--folderPath` is the provider path. In the example above that would be `reg-modules/key-vault` and `reg-modules/web`.
