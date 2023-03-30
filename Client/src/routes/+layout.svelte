<script lang="ts">
    import { page } from '$app/stores'
    import { Form, Modal } from '$components'
    import { API_URL } from '$lib/consts'
    import type { IUser, ModalComponent } from '$lib/types'
    import dayjs from 'dayjs'
    import 'dayjs/locale/ru'
    import { logout } from '$lib/utils'

    dayjs.locale('ru')

    $: URL = $page.url.pathname

    export let data: any
    let user: IUser = data.user

    let loginModal: ModalComponent
    let registerModal: ModalComponent

    interface MenuItem {
        path: string,
        title: string
    }

    const menuItems: MenuItem[] = [
        { title: 'Главная страница', path: '/' },
        { title: 'Новости', path: '/news' },
        { title: 'Категории', path: '/categories' },
    ]

    const registerHandler = () => {
        setTimeout(() => registerModal.close(), 1500)
        loginModal.open()
    }

    const loginHandler = () => {
        setTimeout(() => loginModal.open(), 2500)
    }
</script>

<div class="container">
    <header class="d-flex flex-column align-items-center">
        <div class="d-flex align-items-center py-3">
            {#if user}
                <p class="mb-0 ms-3">Добро пожаловать, {user.firstName}</p>
                <button type="button" class="btn btn-sm btn-secondary ms-3" on:click={logout}>Выйти</button>
            {:else}
                <button type="button" class="btn btn-sm btn-secondary me-3" on:click={loginModal.open}>Войти</button>
                <button type="button" class="btn btn-sm btn-secondary" on:click={registerModal.open}>Регистрация</button>
            {/if}
        </div>
        <nav class="d-flex justify-content-center py-3 align-items-center">
            <ul class="nav nav-pills">
                {#each menuItems as item, i}
                    <li class="nav-item"><a href={item.path} class="nav-link" class:active={ i == 0 ? URL == item.path : URL.startsWith(item.path) }>{item.title}</a></li>
                {/each}
            </ul>
        </nav>
    </header>
</div>

<main class="pt-5">
    <slot />
</main>

<Modal bind:this={ loginModal } hasFooter={ false } title="Вход">
    <Form method="POST" action="{API_URL}/auth/login" redirectTo="/" on:success={loginHandler} autocomplete={true}>
        <div class="mb-3">
            <label for="email" class="form-label">Email</label>
            <input type="email" name="email" class="form-control" required/>
        </div>
        <div class="mb-3">
            <label for="password" class="form-label">Пароль</label>
            <input type="password" name="password" class="form-control" required/>
        </div>
        <button class="btn btn-primary mb-3">Войти</button>
    </Form>
</Modal>

<Modal bind:this={ registerModal } hasFooter={ false } title="Регистрация">
    <Form method="POST" action="{API_URL}/auth/signup" on:success={registerHandler} autocomplete={true}>
        <div class="mb-3">
            <label for="firstName" class="form-label">Имя</label>
            <input type="text" name="firstName" class="form-control" required/>
        </div>
        <div class="mb-3">
            <label for="lastName" class="form-label">Фамилия</label>
            <input type="text" name="lastName" class="form-control" required/>
        </div>
        <div class="mb-3">
            <label for="email" class="form-label">Email</label>
            <input type="email" name="email" class="form-control" required/>
        </div>
        <div class="mb-3">
            <label for="password" class="form-label">Пароль</label>
            <input type="password" name="password" class="form-control" required/>
        </div>
        <button class="btn btn-primary mb-3">Регистрация</button>
    </Form>
</Modal>
