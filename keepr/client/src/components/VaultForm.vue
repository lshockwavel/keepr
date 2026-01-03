<script setup>
import { vaultsService } from '@/services/VaultsService.js';
import { Pop } from '@/utils/Pop.js';
import { Modal } from 'bootstrap';
import { ref } from 'vue';
import { useRouter } from 'vue-router';

const router = useRouter();


const vaultForm = ref({
    name: '',
    description: '',
    isPrivate: false,
    img: ''
});


//TODO function to handle vault form submission when I have time.
async function handleSubmit(event) {
    try {
        event.preventDefault();
        // Handle form submission logic here
        console.log('Vault Form Submitted:', vaultForm.value);
        // You can call a service to save the vault data

        const newVault = await vaultsService.createVault(vaultForm.value);

        Pop.toast("Vault created successfully!");


        Modal.getInstance('#vaultModal').hide();

        // await keepsService.getKeeps();

        router.push({ name: 'Vault', params: { vaultId: newVault.data.id } });

        // Reset the form after submission
        resetForm();
    } catch (error) {
        console.error("Error creating vault:", error);
    }

}

function resetForm() {
    vaultForm.value = {
        name: '',
        description: '',
        isPrivate: false,
        img: ''
    };
}

</script>


<template>
    <div class="modal fade" id="vaultModal" tabindex="-1" aria-labelledby="vaultModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="vaultModalLabel">Add your Vault</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <div class="form">
                        <form @submit="handleSubmit">
                            <div class="mb-3">
                                <label for="vault-name" class="form-label">Vault Name</label>
                                <input type="text" class="form-control" id="vault-name" v-model="vaultForm.name"
                                    required />
                            </div>
                            <div class="mb-3">
                                <label for="vault-description" class="form-label">Description</label>
                                <textarea class="form-control" id="vault-description" v-model="vaultForm.description"
                                    rows="3"></textarea>
                            </div>
                            <div class="mb-3 form-check">
                                <input type="checkbox" class="form-check-input" id="vault-private"
                                    v-model="vaultForm.isPrivate" />
                                <label class="form-check-label" for="vault-private">Private Vault</label>
                            </div>
                            <div class="mb-3">
                                <label for="vault-img" class="form-label">Image URL</label>
                                <input type="text" class="form-control" id="vault-img" v-model="vaultForm.img" />
                                <div v-if="vaultForm.img">
                                    <h6 class="mt-2 mb-0">Image Preview:</h6>
                                    <img class="preview-size" v-if="vaultForm.img" :src="vaultForm.img" alt="Preview"/> 
                                </div>
                            </div>
                            <button type="submit" class="btn btn-primary">Create Vault</button>
                        </form>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>


<style lang="scss" scoped>
.preview-size {
    margin-top: 10px;
    border: 1px solid #ddd;
    padding: 5px;
    border-radius: 4px;
    max-width: 200px;
}
</style>