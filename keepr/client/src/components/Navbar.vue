<script setup>
import { computed, ref, watch } from 'vue';
import { loadState, saveState } from '../utils/Store.js';
import Login from './Login.vue';
import KeepForm from './KeepForm.vue';
import VaultForm from './VaultForm.vue';
import { RouterLink } from 'vue-router';
import { AppState } from '@/AppState.js';

const theme = ref(loadState('theme') || 'light')

function toggleTheme() {
  theme.value = theme.value == 'light' ? 'dark' : 'light'
}

watch(theme, () => {
  document.documentElement.setAttribute('data-bs-theme', theme.value)
  saveState('theme', theme.value)
}, { immediate: true })

//Makes sure to clear active keep when opening the keep form
function clearKeepForm() {
  AppState.activeKeep = null;
}

const account = computed(() => AppState.account)

</script>

<template>
  <nav class="navbar navbar-expand-md bg-codeworks border-bottom border-vue">
    <div class="container-fluid gap-2">
      <RouterLink :to="{ name: 'Home' }" class="d-flex align-items-center text-light">
        <b class="fs-5">Home</b>
      </RouterLink>
      <!-- collapse button -->
      <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbar-links"
        aria-controls="navbarText" aria-expanded="false" aria-label="Toggle navigation">
        <span class="mdi mdi-menu text-light"></span>
      </button>
      <!-- collapsing menu -->
      <div class="collapse navbar-collapse " id="navbar-links">
        <ul class="navbar-nav">
          <li v-if="account" class="nav-item dropdown">
            <button class="btn dropdown-toggle" data-bs-toggle="dropdown">
              Create
            </button>
            <ul class="dropdown-menu">
              <li><button class="dropdown-item" data-bs-toggle="modal" data-bs-target="#keepModal" @click="clearKeepForm()">
                  new Keep
                </button></li>
              <li><button class="dropdown-item" data-bs-toggle="modal" data-bs-target="#vaultModal">
                  new Vault
                </button></li>
            </ul>
          </li>
        </ul>
        <!-- Centered title -->
        <h1 class="mx-auto text-light mb-0">Keepr</h1>
        <!-- LOGIN COMPONENT HERE -->
        <div class="d-flex gap-2 align-items-center me-2">
          <button class="btn text-light" @click="toggleTheme"
            :title="`Enable ${theme == 'light' ? 'dark' : 'light'} theme.`">
            <i v-if="theme == 'dark'" class="mdi mdi-weather-sunny"></i>
            <i v-if="theme == 'light'" class="mdi mdi-weather-night"></i>
          </button>
        </div>
        <Login />
      </div>
    </div>
  </nav>
  <KeepForm />
  <VaultForm />
</template>

<style lang="scss" scoped>
a {
  text-decoration: none;
}

.nav-link {
  text-transform: uppercase;
}

.navbar-nav .router-link-exact-active {
  border-bottom: 2px solid var(--bs-success);
  border-bottom-left-radius: 0;
  border-bottom-right-radius: 0;
}
</style>
