<script setup>
import { AppState } from '@/AppState.js';
import { Account } from '@/models/Account.js';
import { KeepVault } from '@/models/Keep.js';
import { VaultKeep } from '@/models/VaultKeep.js';
import { vaultsService } from '@/services/VaultsService.js';
import { computed } from 'vue';



const ActiveKeepVault = computed(() => AppState.activeKeepVault);

const props = defineProps({
    vaultId : { type: Number, required: true }, 
    keepVault: { type: KeepVault, required: true },
    account: { type: Account, required: false } });


async function getActiveVaultKeepByVaultIdAndVaultKeepId(vaultId, vaultKeepId) {
    try {
        await vaultsService.getVaultKeepByVaultIdAndVaultKeepId(vaultId, vaultKeepId);
        console.log("Active vaultKeep set to:", AppState.activeVaultKeep);
    } catch (error) {
        console.error("Error setting active vaultKeep:", error);
    }
    }

</script>


<template>
    <div class="card h-100 keep-card-image" data-bs-toggle="modal" data-bs-target="#vault-keep-modal" @click="getActiveVaultKeepByVaultIdAndVaultKeepId(vaultId, keepVault.vaultKeepId)">
        <img :src="keepVault.img" :alt="keepVault.name" class="card-img-top" />
        <h5 class="keep-name">{{ keepVault.name }}</h5>
    </div>
</template>


<style lang="scss" scoped>

    .keep-card-image {
  position: relative; /* This makes it the positioning context */
  cursor: pointer;
  
}

.keep-name {
  position: absolute;
  bottom: 10px;  /* Distance from bottom */
  left: 10px;    /* Distance from left */
  
  /* Make text readable over image */
  color: white;
  background-color: rgba(0, 0, 0, 0.6); /* Semi-transparent background */
  padding: 0.5rem;
  margin: 0;
}

</style>