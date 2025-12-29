export class VaultKeep{
    constructor(data) {
        this.id = data.id;
        this.createdAt = new Date(data.createdAt);
        this.updatedAt = new Date(data.updatedAt);
        this.creatorId = data.creatorId;
        this.vaultId = data.vaultId;
        this.keepId = data.keepId;
    }
}