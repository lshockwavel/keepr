<script setup >
import { computed, onMounted, watch } from "vue";
import { AppState } from "@/AppState.js";
import { keepsService } from "@/services/KeepsService.js";
import KeepCard from "@/components/KeepCard.vue";
import KeepModal from '@/components/KeepModal.vue';

const keeps = computed(() => AppState.keeps);

const account = computed(() => AppState.account);

onMounted( () => {
  getKeeps();
} )

//If a keep is added/removed, refresh the keeps list
watch(keeps, () => {
  console.log("Keeps updated:", keeps.value);
  getKeeps();
});

async function getKeeps() {
  try {
    await keepsService.getKeeps();
  } catch (error) {
    console.error("Error fetching keeps:", error);
  }
}

</script>

<template>
  <div class="container-fluid">
    <section class="row">
      <div>
        <h2 class="text-center my-4">Welcome to Keepr</h2>
        <div class="masonry-grid">
          <div v-for="keep in keeps" :key="keep.id">
            <KeepCard :keep="keep" />
          </div>
              <KeepModal />
        </div>
      </div>
    </section>
  </div>
</template>

<style scoped lang="scss">

// .masonry-grid {
//   display: grid;
//   grid-template-columns: repeat(auto-fill, minmax(250px, 1fr));
//   gap: 1rem;
// }

// .masonry-grid {
//   column-count: 4;
//   column-gap: 1rem;
// }

.masonry-grid {
  column-count: 4;
  column-gap: 1rem;
  
  > div {
    break-inside: avoid; // Prevents cards from breaking across columns
    margin-bottom: 1rem;
  }
}

// Responsive breakpoints
@media (max-width: 1200px) {
  .masonry-grid { column-count: 3; }
}
@media (max-width: 768px) {
  .masonry-grid { column-count: 2; }
}
@media (max-width: 480px) {
  .masonry-grid { column-count: 1; }
}
</style>
