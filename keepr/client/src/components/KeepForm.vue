<script setup>
import { AppState } from '@/AppState.js';
import { keepsService } from '@/services/KeepsService.js';
import { ref, watch } from 'vue';



const keepForm = ref({
    name: '',
    description: '',
    img: ''
});

//TODO Maybe use a watcher to update the form when activeKeep changes if I have time.
watch(() => AppState.activeKeep, (newKeep) => {
    if (newKeep) {
        keepForm.value = {
            name: newKeep.name || '',
            description: newKeep.description || '',
            img: newKeep.img || ''
        };
    } else {
        keepForm.value = {
            name: '',
            description: '',
            img: ''
        };
    }
}, { immediate: true });


//TODO part of the edit functionality if I have time.
async function handleSubmit(event) {
    event.preventDefault();
    // Handle form submission logic here
    console.log('Keep Form Submitted:', keepForm.value);
    // You can call a service to save the keep data

    await keepsService.createKeep(keepForm.value);
    // Reset the form after submission
    resetForm();

}


function resetForm() {
    keepForm.value = {
        name: '',
        description: '',
        img: ''
    };
}

</script>


<template>
    <div class="modal fade" id="keepModal" tabindex="-1" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    Add your keep
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <form @submit="handleSubmit">
                        <!-- Form fields for Keep go here -->
                        <div class="mb-3">
                            <label for="keepName" class="form-label">Keep Name</label>
                            <input type="text" class="form-control" id="keepName" v-model="keepForm.name" required />
                        </div>
                        <div class="mb-3">
                            <label for="keepDescription" class="form-label">Description</label>
                            <textarea class="form-control" id="keepDescription" v-model="keepForm.description" rows="3"></textarea>
                        </div>
                        <div class="mb-3">
                            <label for="keepImg" class="form-label">Image URL</label>
                            <input type="text" class="form-control" id="keepImg" v-model="keepForm.img" />
                        </div>
                        <button type="submit" class="btn btn-primary">Save Keep</button>
                    </form>
                </div>
            </div>
        </div>
    </div>
</template>


<style lang="scss" scoped>

</style>