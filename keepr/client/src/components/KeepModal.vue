<script setup>
import { computed, ref, watch } from 'vue';
import { AppState } from '@/AppState.js';
import { accountService } from '@/services/AccountService.js';
import { vaultKeepsService } from '@/services/VaultKeepsService.js';
import { Pop } from '@/utils/Pop.js';
import { Modal } from 'bootstrap';

const activeKeep = computed(() => AppState.activeKeep);

const account = computed(() => AppState.account);

const vaults = computed(() => AppState.vaults); //TODO Update this to use a sep vault appState


watch(account, (newAccount) => {
  if (newAccount) {
    getVaultsByAccountId();
  }
}, { immediate: true });

const selectedVault = ref("");

async function getVaultsByAccountId() {

  console.log("Fetching vaults for account:", account?.value.id);

  if (!account.value) {
    console.warn("No account is currently logged in.");
    return;
  }
  try {
    // Assuming you have a vaultService similar to keepsService
    await accountService.GetAccountVaults();
    console.log("Vaults fetched for account:", account?.value.id);
  } catch (error) {
    console.error("Error fetching vaults:", error);
  }
}

async function addKeepToVault(vaultId) {
  try {
    // Assuming you have a vaultsService with an addKeepToVault method
    const vaultKeep = {
      keepId: activeKeep.value.id,
      vaultId: vaultId
    };

    console.log("Adding Keep to Vault:", selectedVault.value);
    await vaultKeepsService.addKeepToVault(vaultKeep);
    console.log(`Keep ${activeKeep.value.id} added to Vault ${vaultId}`);

    //Reset selected vault
    selectedVault.value = "";

    Pop.toast("Keep added to vault!");
    Modal.getInstance('#keep-modal').hide();

  } catch (error) {
    Pop.error(error);
    console.error("Error adding keep to vault:", error);
  }
}



</script>


<template>
  <div class="modal fade hover" id="keep-modal" tabindex="-1">
    <div class="modal-dialog modal-xl">
      <div v-if="activeKeep" class="modal-content">
        <!-- <div class="modal-header">
                    <h5 class="modal-title">{{ activeKeep?.name }}</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <p>Keep modal content goes here...</p>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>
                </div> -->
        <div class="modal-body">
          <div class="row">
            <div class="col-6">
              <img :src="activeKeep?.img" class="img-fluid" alt="description">
            </div>
            <div class="col-6">
              <section class="row">
                <div class="col-12 text-end">
                  <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
              </section>
              <section class="row">
                <div class="col-12 text-center">
                  <div class="d-flex">
                    <i class="mdi mdi-eye"><span class="me-2">Views: {{ activeKeep?.views }}</span></i>
                    <i class="mdi mdi-alpha-k-box-outline"><span class="me-2">Kept: {{ activeKeep?.kept }}</span></i>
                  </div>
                </div>
              </section>
              <section class="row">
                <div class="d-flex justify-content-center align-items-center"></div>
                <div>
                  <h2>{{ activeKeep?.name }}</h2>
                  <p>{{ activeKeep?.description }}</p>
                  <hr />
                  <h5>Created by:</h5>
                  <div class="d-flex align-items-center">
                    <img :src="activeKeep?.creator.picture" :alt="activeKeep?.creator.name" class="rounded-circle me-2"
                      style="width: 50px; height: 50px;" />
                    <span>{{ activeKeep?.creator.name }}</span>
                  </div>
                </div>
                <div class="d-flex">
                  <!-- Example select dropdown -->
                   <div v-if="!account">
                    <p>Please log in to add this keep to a vault.</p>
                  </div>
                  <div v-else-if="vaults.length === 0">No vaults available</div>
                  <div v-else>
                    <select v-model="selectedVault" class="form-select mt-3">
                      <option value="">Select an option...</option>
                      <option v-for="vault in vaults" :key="vault.id" :value="vault.id">
                        {{ vault.name }}
                      </option>
                    </select>
                    <button class="btn btn-primary mt-3" @click="addKeepToVault(selectedVault)">Save</button>
                  </div>
                </div>
              </section>
            </div>
          </div>
        </div>
      </div>
      <div v-else class="modal-content">
        <div class="skeleton">
          <div class="skeleton-header"></div>
          <div class="skeleton-body"></div>
          <div class="skeleton-footer"></div>
        </div>
      </div>
    </div>
  </div>
</template>


<style lang="scss" scoped>
.skeleton {
  animation: pulse 1.5s infinite;

  .skeleton-header,
  .skeleton-body,
  .skeleton-footer {
    background: #e0e0e0;
    border-radius: 4px;
    margin: 1rem;
  }

  .skeleton-header {
    height: 2rem;
    width: 50%;
  }

  .skeleton-body {
    height: 10rem;
    width: 100%;
  }

  .skeleton-footer {
    height: 2rem;
    width: 30%;
  }
}

@keyframes pulse {
  0% {
    opacity: 1;
  }

  50% {
    opacity: 0.4;
  }

  100% {
    opacity: 1;
  }
}
</style>