<script setup>
import { AppState } from '@/AppState.js';
import { Account } from '@/models/Account.js';
import { Keep } from '@/models/Keep.js';
import { router } from '@/router.js';
import { keepsService } from '@/services/KeepsService.js';
import { Pop } from '@/utils/Pop.js';
import { Modal } from 'bootstrap';
import { computed } from 'vue';

const activeKeep = computed(() => AppState.activeKeep);
const activeAccount = computed(() => AppState.account);

const props = defineProps({
  keep: { type: Keep, required: true },
  account: { type: Account, required: false }
})

async function getActiveKeep(keepId) {
  try {
    await keepsService.getKeepById(keepId);
    console.log("Active keep set to:", AppState.activeKeep);

    //Get the modal to show the keep details. This will be used so the Delete button works from the KeepCard
    const modalElement = document.querySelector('#keep-modal');
    if (modalElement) {
      Modal.getOrCreateInstance(modalElement).show();
    }
  } catch (error) {
    console.error("Error setting active keep:", error);
  }
}

async function deleteKeep(keepId) {
  try {
    const confirmDelete = confirm("Are you sure you want to delete this keep?");
    if (!confirmDelete) {
      return;
    }

    await keepsService.deleteKeep(keepId);
    console.log("Keep deleted:", keepId);

    Pop.toast("Keep deleted successfully!", "success");

    // Optionally, you can refresh the keeps list or update the UI accordingly
    // await keepsService.getKeeps(); 
    // Since this will be used in more than one place, we will let the page handle the refresh

  } catch (error) {
    console.error("Error deleting keep:", error);
  }
}



</script>


<template>
  <div class="card h-100 keep-card-image" @click="getActiveKeep(keep.id)">
    <i v-if="activeAccount?.id == keep.creatorId" class="mdi mdi-delete delete-icon"
      @click.stop="deleteKeep(keep.id)"></i>
    <img :src="keep.img" class="card-img-top" :alt="keep.name" />
    <h5 class="keep-name">{{ keep.name }}</h5>
    <router-link :to="{ name: 'Profile', params: { profileId: keep.creatorId } }" @click.stop>
      <img :src="keep.creator.picture" :alt="keep.creator.name" class="keep-creator-picture m-2" />
    </router-link>
  </div>
  <!-- <KeepModal /> -->
</template>


<style lang="scss" scoped>
.keep-card-image {
  position: relative;
  /* This makes it the positioning context */
  cursor: pointer;
}

.delete-icon {
  position: absolute;
  top: 10px;
  right: 10px;
  color: red;
  background-color: white;
  border-radius: 50%;
  padding: 5px;
  cursor: pointer;
  z-index: 10;

  &:hover {
    background-color: #ffcccc;

  }
}

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

.keep-creator-picture {
  width: 40px;
  height: 40px;
  border: 2px solid white;
}

.keep-creator-picture {
  width: 40px;
  height: 40px;
  border: 2px solid white;
  border-radius: 50%;
  position: absolute;
  bottom: 0;
  right: 0;
  //   margin: 0.5rem;
}
</style>