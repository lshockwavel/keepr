<script setup>
import Navbar from './components/Navbar.vue';
import { RouterView } from 'vue-router';
import KeepModal from '@/components/KeepModal.vue';

</script>

<template>
  <header>
    <Navbar />
  </header>
  <main>
    <RouterView/>
    <KeepModal />
  </main>
  <!-- <footer class=" text-center">
    Made with <i class="mdi mdi-heart text-pink"></i> by CodeWorks
  </footer> -->
</template>

<style lang="scss">


</style>
