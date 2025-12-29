export class Profile {
  /**
   * @typedef ProfileData
   * @property {string} id
   * @property {string} email
   * @property {string} name
   * @property {string} picture
   * @property {Date|string} createdAt
   * @property {Date|string} updatedAt
   * @property {string} coverImg
   * 
   * @param {ProfileData} data
   */
  constructor(data) {
    this.id = data.id
    this.name = data.name
    this.picture = data.picture
    this.createdAt = new Date(data.createdAt)
    this.updatedAt = new Date(data.updatedAt)
    this.coverImg = data.coverImg || 'https://images.unsplash.com/photo-1584951899892-ab77edd77a0e?q=80&w=1169&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D'
  }
}


export class Account extends Profile {
  /**
   * @typedef AccountData
   * @property {string} id
   * @property {string} name
   * @property {string} picture
   * @property {Date|string} createdAt
   * @property {Date|string} updatedAt
   * @property {string} coverImg
   * @property {string} email
   * 
   * @param {AccountData} data
   */
  constructor(data) {
    super(data)
    this.email = data.email
  }
}