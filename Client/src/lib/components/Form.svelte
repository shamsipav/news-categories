<script lang="ts">
    import axios, { type AxiosResponse } from 'axios'
    import { createEventDispatcher } from 'svelte'
    import { blur } from 'svelte/transition'
    import type { RESTMethod } from '$lib/types'
    import { getCookie, redirect, setCookie } from '$lib/utils'

    export let id: string = undefined
    export let className = ''
    export let action = ''
    export let reset = true
    export let method: RESTMethod = 'GET'
    export let redirectTo: string = undefined
    export let autocomplete = false
    export let extended = false

    let submitted = false
    let successMessage = ''
    let errorMessage = ''

    const dispatch = createEventDispatcher()

    const handleSubmit = async (event) => {
        const actionUrl = event.target.action

        // Костыль для ответов AuthController
        const route = actionUrl.split('/').slice(-1)[0]
        const controller = actionUrl.split('/').slice(-2)[0]

        const formData = new FormData(event.target)
        const dataObject: any = {}
        formData.forEach((value, key) => dataObject[key] = value)

        const config =
        {
            headers: {
                'Authorization': `Bearer ${getCookie('token')}`
            }
        }


        try {
            let response: AxiosResponse

            if (method == 'POST' || method == 'PUT') {
                if (extended) {
                    const categoriesObject = []
                    const categoriesNames = []
                    const categoriesIds = []
                    Object.keys(dataObject).map((key) => {
                        if (key.startsWith('category_name')) {
                            categoriesNames.push(dataObject[key])
                            delete dataObject[key]
                        }
                        if (key.startsWith('category_id')) {
                            categoriesIds.push(dataObject[key])
                            delete dataObject[key]
                        }
                    })

                    for (let i = 0; i <= categoriesNames.length; i++) {
                        const name = categoriesNames[i]
                        const id = categoriesIds[i]
                        if (id && name) {
                            categoriesObject[i] = {
                                id: id,
                                name: name,
                                news: []
                            }
                        }
                    }

                    dataObject.categories = categoriesObject
                }

                if (method == 'POST')
                    response = await axios.post(actionUrl, dataObject, config)

                if (method == 'PUT')
                    response = await axios.put(actionUrl, dataObject, config)
            }

            if (method == 'DELETE')
                response = await axios.delete(actionUrl, config)

            if (method == 'GET')
                response = await axios.get(actionUrl)

            if (reset)
                event.target.reset()

            if (redirectTo)
                redirect(redirectTo)

            submitted = true
            errorMessage = ''

            if (controller === 'auth' && route === 'login') {
                setCookie('token', response.data, 3, true)
                successMessage = 'Вход выполнен'
            } else {
                successMessage = response.data.message
            }

            if (controller == 'auth' && route !== 'login') {
                successMessage = response.data
            }

            dispatch('success', response.data)

        } catch (error) {
            submitted = true
            successMessage = ''
            errorMessage = error.response.data
            console.log(`Не удалось выполнить запрос: ${error.response.data}`)
        }
    }
</script>

<form {id} class={className} {action} {method} on:submit|preventDefault={handleSubmit} autocomplete={autocomplete ? 'on' : 'off'}>
    <slot />
    {#if submitted}
        <div class="alerts mt-3 mb-3" transition:blur|local={{ duration: 200 }}>
            {#if successMessage}
                <div transition:blur|local={{ duration: 200 }} class="alert alert-success mb-0" role="alert">
                    {successMessage}
                </div>
            {:else if errorMessage}
                <div transition:blur|local={{ duration: 200 }} class="alert alert-danger mb-0" role="alert">
                    {errorMessage}
                </div>
            {/if}
        </div>
    {/if}
</form>

<style>
    div.alert {
        color: black;
    }
</style>
