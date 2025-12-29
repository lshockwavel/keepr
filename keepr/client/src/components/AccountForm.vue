<script setup>
import { AppState } from '@/AppState.js';
import { accountService } from '@/services/AccountService.js';
import { Modal } from 'bootstrap';

import { computed, onMounted, ref, watch } from 'vue';

const account = computed(() => AppState.account);

const accountForm = ref({
    name: '',
    picture: '',
    coverImg: ''
});

watch(() => account.value, (newAccount) => {
    if (newAccount) {
        accountForm.value.name = newAccount.name || '';
        accountForm.value.picture = newAccount.picture || '';
        accountForm.value.coverImg = newAccount.coverImg || '';
    }
}, { immediate: true });

onMounted(() => {

});

function resetForm() {
    accountForm.value = {
        name: '',
        picture: '',
        coverImg: ''
    };
}

async function getAccountInfo() {
    try {
        await accountService.getAccount();
        console.log("Account info refreshed:", AppState.account);
    } catch (error) {
        console.error("Error refreshing account info:", error);
    }
}

async function handleSubmit(event) {

    try {
        event.preventDefault();
        // Handle form submission logic here
        console.log('Account Form Submitted:', accountForm.value);
        // You can call a service to save the account data
        await accountService.updateAccount(accountForm.value);
    
        // Reset the form after submission
        resetForm();

        Modal.getOrCreateInstance("#account-form-modal").hide();

        

    } catch (error) {
        console.error("Error updating account:", error);
    }
}

</script>


<template>
    <div class="modal fade" id="account-form-modal" tabindex="-1" aria-labelledby="accountFormModalLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="accountFormModalLabel">Manage Account</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <form action="" @submit="handleSubmit">
                        <div class="mb-3">
                            <label for="name" class="form-label">Name</label>
                            <input type="text" id="name" v-model="accountForm.name" class="form-control" required />
                        </div>
                        <div class="mb-3">
                            <label for="picture" class="form-label">Profile Picture URL</label>
                            <input type="text" id="picture" v-model="accountForm.picture" class="form-control" />
                        </div>
                        <div class="mb-3">
                            <label for="coverImg" class="form-label">Cover Image URL</label>
                            <input type="text" id="coverImg" v-model="accountForm.coverImg" class="form-control" />
                        </div>
                        <button type="submit" class="btn btn-primary">Save Changes</button>
                    </form>
                </div>
            </div>
        </div>
    </div>
</template>


<style lang="scss" scoped></style>