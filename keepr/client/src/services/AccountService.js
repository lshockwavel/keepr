import { Vault } from '@/models/Vault.js'
import { AppState } from '../AppState.js'
import { Account } from '../models/Account.js'
import { logger } from '../utils/Logger.js'
import { api } from './AxiosService.js'

class AccountService {
  async updateAccount(value) {
    try {
      console.log('Updating account with value:', value)
      const response = await api.put('/account', value)

      console.log('updateAccount response', response)
      AppState.account = new Account(response.data)
    } catch (error) {
      logger.error('Error updating account', error)
    }
  }
  async getAccount() {
    try {
      const res = await api.get('/account')
      AppState.account = new Account(res.data)
    } catch (err) {
      logger.error('HAVE YOU STARTED YOUR SERVER YET???', err)
    }
  }

  async GetAccountVaults() {
    try {
      const response = await api.get('/account/vaults')
      
      logger.log('account vaults', response.data)
      AppState.vaults = response.data.map(vault => new Vault(vault))
    } catch (error) {
      logger.error('Error getting account vaults', error)
    }
  }
}

export const accountService = new AccountService()
