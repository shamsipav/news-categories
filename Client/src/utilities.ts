export const postData = async (url = '', data = {}) => {
    const response = await fetch(url, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(data),
    })
    return response.json()
}

const redirectDelay = 500
export const redirect = (location: string) => setTimeout(() => { window.location.href = location }, redirectDelay)

export async function logout() {
    const response = await fetch('http://localhost:8080/api/auth/logout')

    if (response.ok) {
        redirect('/')
        console.log('LOGOUT SUCCESS', response)

    } else {
        console.log('Ошибка HTTP: ' + response.status)
    }
}