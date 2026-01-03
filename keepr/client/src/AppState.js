import { reactive } from 'vue'
import { Keep, KeepVault } from './models/Keep.js'
import { Vault } from './models/Vault.js'
import { Profile } from './models/Account.js'
import { VaultKeep } from './models/VaultKeep.js'

// NOTE AppState is a reactive object to contain app level data
export const AppState = reactive({
  /**@type {import('@bcwdev/auth0provider-client').Identity} */
  identity: null,
  /** @type {import('./models/Account.js').Account} user info from the database*/
  account: null,

  /** @type {Keep[]} */
  keeps: [],

  /** @type {Keep[]} */
  profileKeeps: [],

  /** @type {Vault[]} */
  vaults: [],

  /** @type {Vault[]} */
  profileVaults: [],

  /** @type {Vault} */
  activeVault: null,

  /** @type {Keep} */
  activeKeep: null,

  /** @type {Profile} */
  profile: null,

  /** @type {Profile} */
  activeProfile: null,

  /** @type {KeepVault} */
  activeKeepVault: null,

  /** @type {KeepVault[]} */
  keepVaults: [],

  /** @type {VaultKeep} */
  activeVaultKeep: null, //TODO - check usage and implement if needed

  /** @type {VaultKeep[]} */
  vaultKeeps: []
})

