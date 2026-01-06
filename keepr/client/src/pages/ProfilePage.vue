<script setup>
import { AppState } from '@/AppState.js';
import { profilesService } from '@/services/ProfilesService.js';
import { Pop } from '@/utils/Pop.js';
import { computed, watch } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import KeepCard from '@/components/KeepCard.vue';
import AccountForm from '@/components/AccountForm.vue';
import { accountService } from '@/services/AccountService.js';
import { Account } from '@/models/Account.js';
import VaultCard from '@/components/VaultCard.vue';

const account = computed(() => AppState.account);
const profile = computed(() => AppState.profile);

const keeps = computed(() => AppState.profileKeeps);
const vaults = computed(() => AppState.profileVaults);

// const newKeeps = computed(() => AppState.keeps); // For testing keep updates

const route = useRoute();
const router = useRouter();

watch(route, () => {
    getProfileById();
    getKeepsByProfileId();
    getVaultsByProfileId();
}, { immediate: true });

// Watch account changes - only refetch profile
watch(account, () => {
    if (route.params.profileId === account.value?.id) {
        getProfileById(); // Only refetch profile if viewing your own
    }
});

// watch(newKeeps, () => {
//     //Checking the last keep to see if it belongs to this profile
//     console.log('Watcher fired! newKeeps length:', newKeeps.value.length);

//     const lastKeep = newKeeps.value[newKeeps.value.length - 1];

//     console.log('Last keep creatorId:', lastKeep?.creatorId, 'type:', typeof lastKeep?.creatorId);
//     console.log('Route profileId:', route.params.profileId, 'type:', typeof route.params.profileId);

//     if (lastKeep && lastKeep.creatorId === account.value?.id) {
//         AppState.profileKeeps.push(lastKeep);
//     }
// });

async function getProfileById() {
    try {
        const profileId = route.params.profileId;
        // Assuming you have a profileService similar to keepsService
        await profilesService.getProfileByProfileId(profileId);
        console.log("Active profile set to:", AppState.profile);
    } catch (error) {
        console.error("Error setting active profile:", error);
        Pop.toast("Profile not found.");
        router.push({ name: 'HomePage' });
    }
}

async function getKeepsByProfileId() {
    try {
        const profileId = route.params.profileId;
        // Assuming you have a keepsService method to get keeps by profile ID
        await profilesService.getKeepsByProfileId(profileId);
        console.log("Keeps fetched for profile:", profileId);
    } catch (error) {
        console.error("Error fetching keeps for profile:", error);
    }
}

async function getVaultsByProfileId() {
    try {
        const profileId = route.params.profileId;
        // Assuming you have a profilesService method to get vaults by profile ID
        await profilesService.getVaultsByProfileId(profileId);
        console.log("Vaults fetched for profile:", profileId);
    } catch (error) {
        console.error("Error fetching vaults for profile:", error);
    }
}

</script>


<template>
    <section class="profile-page container mt-4 mb-5">
        <div class="profile-header text-center mb-4">
            <div class="position-relative mb-3">
                <img :src="profile?.coverImg" :alt="profile?.name" class="profile-cover" />
                <img :src="profile?.picture" :alt="profile?.name" class="profile-picture" />
                <div class="text-end">
                    <i v-if="account?.id == profile?.id" class="mdi mdi-pencil profile-edit-icon mb-2"
                        data-bs-toggle="modal" data-bs-target="#account-form-modal">Edit Profile</i>
                        <AccountForm/>
                </div>
            </div>
            <div class="text-center mt-3">
                <h2 class="profile-name">{{ profile?.name }}</h2>
                <span> {{ vaults.length }} Vaults | {{ keeps.length }} Keeps</span>
            </div>
        </div>

        <div class="profile-vaults mb-5">
            <h3>Vaults</h3>
            <div v-if="vaults.length" class="row">
                <section class="masonry-grid">
                    <div v-for="vault in vaults" :key="vault.id" class="col">
                        <!-- <router-link :to="{ name: 'Vault', params: { vaultId: vault.id } }"
                            class="text-decoration-none">
                            <div class="card h-100">
                                <img :src="vault.img" class="card-img-top" :alt="vault.name" />
                                <div class="card-body">
                                    <i v-if="vault.isPrivate && account.id == vault.creatorId" class="mdi mdi-lock"></i>
                                    <h5 class="card-title">{{ vault.name }}</h5>
                                    <p class="card-text text-truncate">{{ vault.description }}</p>
                                </div>
                            </div>
                        </router-link> -->
                        <VaultCard :vault="vault" />
                    </div>
                </section>
            </div>
            <div v-else class="text-center">
                <p>No vaults found for this profile.</p>
            </div>
        </div>

        <div class="profile-keeps mb-5">
            <h3>Keeps</h3>
            <div class="row">
                <section class="masonry-grid">
                    <div v-for="keep in keeps" :key="keep.id" class="col">
                        <KeepCard :keep="keep" :account="account" />
                    </div>
                </section>
            </div>
        </div>
    </section>
</template>


<style lang="scss" scoped>
.profile-cover {
    width: 100%;
    height: 400px;
    object-fit: cover;
    border-radius: 10px;
    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
    // position: relative;
}

.profile-picture {
    width: 150px;
    height: 150px;
    border-radius: 50%;
    object-fit: cover;
    border: 3px solid #007bff;
    // position: absolute;
    // left: 50%;
    // transform: translateX(-50%);
    // bottom: -75px;
    margin-top: -75px;
}

.profile-edit-icon {
    font-size: 1.2rem;
    cursor: pointer;
    color: #007bff;
}

.masonry-grid {
    column-count: 4;
    column-gap: 1rem;

    >div {
        break-inside: avoid; // Prevents cards from breaking across columns
        margin-bottom: 1rem;
    }
}

// Responsive breakpoints
@media (max-width: 1200px) {
    .masonry-grid {
        column-count: 3;
    }
}

@media (max-width: 768px) {
    .masonry-grid {
        column-count: 2;
    }
}

@media (max-width: 480px) {
    .masonry-grid {
        column-count: 1;
    }
}
</style>