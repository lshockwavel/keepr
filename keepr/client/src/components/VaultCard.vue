<script setup>
import { AppState } from '@/AppState.js';
import { Vault } from '@/models/Vault.js';
import { computed } from 'vue';

const account = computed(() => AppState.account);

const props = defineProps({
    vault: { type: Vault, required: true }
});

</script>


<template>
    <router-link :to="{ name: 'Vault', params: { vaultId: vault.id } }" class="text-decoration-none">
        <div class="card h-100 vault-card">
            <img :src="vault.img" class="card-img-top" :alt="vault.name" />
            <div>
                <i v-if="vault.isPrivate && account.id == vault.creatorId" class="mdi mdi-lock"></i>
                <h5 class="card-title">{{ vault.name }}</h5>
            </div>
        </div>
    </router-link>
</template>


<style lang="scss" scoped>

.vault-card {
  transition: transform 0.2s;
  cursor: pointer;
  position: relative;
}

.vault-card:hover {
  transform: scale(1.05);
}

.vault-card .card-title {
    position: absolute;
    bottom: 10px;
    left: 10px;
    color: white;
    background-color: rgba(0, 0, 0, 0.6);
    padding: 0.5rem;
    margin: 0;
    text-shadow: 1px 1px 2px black;
}

.vault-card i {
    position: absolute;
    top: 10px;
    right: 10px;
    color: white;
    background-color: rgba(0, 0, 0, 0.6);
    padding: 0.3rem;
    border-radius: 4px;
}




</style>