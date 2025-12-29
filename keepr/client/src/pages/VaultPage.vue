<script setup>
import { computed, watch } from 'vue';
import { AppState } from '@/AppState.js';
import { useRoute, useRouter } from 'vue-router';
import { vaultsService } from '@/services/VaultsService.js';
import VaultKeepCard from '@/components/VaultKeepCard.vue';
import KeepModal from '@/components/KeepModal.vue';
import { Vault } from '@/models/Vault.js';
import VaultKeepModal from '@/components/VaultKeepModal.vue';
import { Pop } from '@/utils/Pop.js';


const vault = computed(() => AppState.activeVault);
const account = computed(() => AppState.account);
// const keeps = computed(() => AppState.keeps);
const vaultKeeps = computed(() => AppState.vaultKeeps);
const keepVaults = computed(() => AppState.keepVaults);

const route = useRoute();
const router = useRouter();

watch(route, () => {
    getVaultById();
    getVaultKeepsByVaultId();
}, { immediate: true });

async function getVaultById() {
    try {
        const vaultId = route.params.vaultId;
        // Assuming you have a vaultService similar to keepsService
        await vaultsService.getVaultById(vaultId);
        console.log("Active vault set to:", AppState.activeVault);
    } catch (error) {
        console.error("Error setting active vault:", error);
        Pop.error("Vault not found.", "error");
        router.push({ name: 'Home' });
    }
}

async function getKeepsByVaultId() {
    try {
        const vaultId = route.params.vaultId;
        // Assuming you have a vaultService similar to keepsService
        await vaultsService.getKeepsInVault(vaultId);
        console.log("Keeps fetched for vault:", vaultId);
    } catch (error) {
        console.error("Error fetching keeps for vault:", error);
    }
}

async function getVaultKeepsByVaultId() {
    try {
        const vaultId = route.params.vaultId;
        // Assuming you have a vaultsService similar to keepsService
        await vaultsService.getVaultKeepsByVaultId(vaultId);
        console.log("VaultKeeps fetched for vault:", vaultId);
    } catch (error) {
        console.error("Error fetching vaultKeeps for vault:", error);
    }
}

async function changePrivateStatus() {
    try {
        const vaultId = route.params.vaultId;
        const updatedVault = {
            ...vault.value,
            isPrivate: !vault.value.isPrivate
        };
        await vaultsService.updateVault(vaultId, updatedVault);
        console.log(`Vault ${vaultId} privacy status changed to:`, updatedVault.isPrivate);
        Pop.toast("Vault privacy status updated.", "success");
    } catch (error) {
        console.error("Error updating vault privacy status:", error);
    }
}

async function deleteVault() {
    try {
        const confirmDelete = confirm("Are you sure you want to delete this vault? This action cannot be undone.");
        if (!confirmDelete) {
            return;
        }
        const vaultId = route.params.vaultId;
        await vaultsService.deleteVault(vaultId);
        console.log(`Vault ${vaultId} deleted`);
        router.push({ name: 'Profile', params: { profileId: account.value.id } });
        Pop.toast("Vault deleted successfully.", "success");
    } catch (error) {
        console.error("Error deleting vault:", error);
    }
}


</script>


<template>
    <div v-if="vault" class="container">
        <section class="row justify-content-center my-4">
            <div class="col-12 text-center img-fluid">
                <div class="position-relative d-inline-block">
                    <img :src="vault.img" :alt="vault.name" class="vault-image mb-3" />
                    <h2 class="vault-name">{{ vault.name }}</h2>
                    <p class="creator-name">Created by: {{ vault.creator.name }}</p>
                    <div v-if="vault.isPrivate" class="mdi mdi-lock-outline position-absolute top-0 end-0 m-3">
                    </div>
                </div>
                <p>{{ vault.description }}</p>
            </div>
        </section>
        <section class="row text-center mb-4">
            <div class="d-flex justify-content-evenly align-items-center gap-3">
                <span>
                    {{ vaultKeeps.length }} Keeps
                </span>
                <div v-if="account.id == vault.creatorId" class="dropdown">
                    <button class="btn" type="button" data-bs-toggle="dropdown">
                        <i class="mdi mdi-dots-horizontal"></i>
                    </button>
                    <ul class="dropdown-menu">
                        <li>
                            <a class="dropdown-item" @click="changePrivateStatus">
                                {{ vault.isPrivate ? 'Make Public' : 'Make Private' }}
                            </a>
                        </li>
                        <li>
                            <a class="dropdown-item text-danger" @click="deleteVault">
                                Delete Vault
                            </a>
                        </li>
                    </ul>
                </div>
            </div>
        </section>
        <section class="row">
            <div v-if="vaultKeeps.length === 0" class="col-12 text-center">
                <p>No keeps found in this vault.</p>
            </div>
            <div v-else class="col-12">
                <div class="masonry-grid">
                    <!-- <div v-for="keep in keeps" :key="keep.id" class="mb-4">
                        <div class="card h-100 keep-card-image">
                            <img :src="keep.img" class="card-img-top" :alt="keep.name" />
                            <h5 class="keep-name">{{ keep.name }}</h5>
                        </div>
                    </div> -->
                    <div v-for="keepVault in keepVaults" :key="keepVault.id">
                        <VaultKeepCard :vaultId="vault.id" :keepVault="keepVault" />
                    </div>
                    <VaultKeepModal />
                </div>
            </div>
        </section>
    </div>
</template>


<style lang="scss" scoped>
.vault-image {
    max-width: 100%;
    height: auto;
    border-radius: 8px;
    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
}

.masonry-grid {
    column-count: 4;
    column-gap: 1rem;

    >div {
        break-inside: avoid; // Prevents cards from breaking across columns
        margin-bottom: 1rem;
    }
}

// .keep-card-image {
//     position: relative;
//     /* This makes it the positioning context */
//     cursor: pointer;
// }

.keep-name {
    position: absolute;
    bottom: 10px;
    /* Distance from bottom */
    left: 10px;
    /* Distance from left */

    /* Make text readable over image */
    color: white;
    background-color: rgba(0, 0, 0, 0.6);
    /* Semi-transparent background */
    padding: 0.5rem;
    margin: 0;
}

.vault-name {
    position: absolute;
    bottom: 10px;
    left: 10px;
    color: white;
    background-color: rgba(0, 0, 0, 0.2);
    padding: 0.5rem;
}

.creator-name {
    position: absolute;
    bottom: 50px;
    left: 10px;
    color: white;
    padding: 0.5rem;
    text-shadow: 1px 1px 2px black;
}

 .dropdown-item {
    cursor: pointer;
 }
</style>