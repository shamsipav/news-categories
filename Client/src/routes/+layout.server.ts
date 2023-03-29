// TODO: Remove axios, add types and refactoring code
import { API_URL } from '$lib/consts'
import axios from 'axios'
import https from 'https'

export async function load({ cookies }) {
    const token = cookies.get('token')

    if (token) {
        try {
            // TODO: Небезопасный код
            const httpsAgent = new https.Agent({ rejectUnauthorized: false })
            const response = await axios.get(`${API_URL}/auth/user`, { headers: { 'Authorization': `Bearer ${token}` }, httpsAgent })

            return {
                user: response.data
            }
        } catch(error) {
            console.log(error.response.message)
        }
    }
}