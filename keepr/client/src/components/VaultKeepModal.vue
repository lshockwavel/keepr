<script setup>
import { AppState } from '@/AppState.js';
import { vaultKeepsService } from '@/services/VaultKeepsService.js';
import { computed, watch } from 'vue';

const account = computed(() => AppState.account);
const activeKeepVault = computed(() => AppState.activeKeepVault);


watch(() => AppState.activeKeepVault, (newKeepVault) => {
    if (newKeepVault) {
        // Handle changes to activeKeepVault if necessary
        console.log("Active VaultKeep changed:", newKeepVault);
    }
}, { immediate: true });

async function deleteVaultKeep() {
    try {
        const confirmDelete = confirm("Are you sure you want to remove this keep from the vault?");
        if (!confirmDelete) {
            return;
        }

        await vaultKeepsService.deleteVaultKeep(activeKeepVault.value.vaultKeepId);

        // Implement deletion logic here
        console.log("Deleting VaultKeep:", activeKeepVault.value);
        // You can call a service to delete the vault keep

        // After deletion, you might want to clear the activeKeepVault
        AppState.activeKeepVault = null;
    } catch (error) {
        console.error("Error deleting vault keep:", error);
    }
}


</script>


<template>
    <div class="modal fade" id="vault-keep-modal" tabindex="-1" aria-labelledby="vaultKeepModalLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div v-if="activeKeepVault" class="modal-content">
                <!-- <div class="modal-header">
                    <h5 class="modal-title" id="vaultKeepModalLabel">Add Keep to Vault</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div> -->
                <div class="modal-body">
                    <div class="row">
                        <div class="col-6">
                            <img :src="activeKeepVault?.img" :alt="activeKeepVault?.name" class="img-fluid" />
                        </div>
                        <div class="col-6">
                            <section class="row">
                                <div class="col-12 text-end">
                                    <button type="button" class="btn-close" data-bs-dismiss="modal"
                                        aria-label="Close"></button>
                                </div>
                            </section>
                            <section class="row my-3">
                                <div class="col-12 text-center">
                                    <div class="d-flex">
                                        <i class="mdi mdi-eye"> <span>{{ activeKeepVault?.views }}</span></i>
                                        <i class="mdi mdi-alpha-k-box-outline"> <span>{{ activeKeepVault?.kept }}</span></i>
                                    </div>
                                </div>
                            </section>
                            <section class="row">
                                <div class="col-12 text-center">
                                    <h3>{{ activeKeepVault?.name }}</h3>
                                    <p>{{ activeKeepVault?.description }}</p>
                                </div>
                            </section>
                            <section class="row">
                                <div class="d-flex">
                                    <button class="btn btn-danger" @click="deleteVaultKeep">Remove from Vault</button>
                                    <div>
                                        <div class="d-flex align-items-center">
                                            <img :src="activeKeepVault?.creator.picture"
                                                :alt="activeKeepVault?.creator.name" class="rounded-circle me-2"
                                                style="width: 50px; height: 50px;" />
                                            <span>{{ activeKeepVault?.creator.name }}</span>
                                        </div>
                                    </div>
                                </div>
                            </section>
                        </div>
                    </div>
                </div>
            </div>
            <div class="modal-content">
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