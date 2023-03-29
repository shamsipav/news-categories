<script lang="ts">
    import { page } from '$app/stores'
    import { Form, Modal } from '$components';
    import { API_URL } from '$lib/consts';
    import type { IUser, ModalComponent } from '$lib/types';
    import dayjs from 'dayjs'
    import 'dayjs/locale/ru'

    dayjs.locale('ru')

    $: URL = $page.url.pathname

    let user: IUser = $page.data.user

    let modal: ModalComponent

    interface MenuItem {
        path: string,
        title: string
    }

    const menuItems: MenuItem[] = [
        { title: 'Главная страница', path: '/' },
        { title: 'Новости', path: '/news' },
        { title: 'Категории', path: '/categories' },
    ]
</script>

<div class="container">
    <header class="d-flex justify-content-center py-3">
        <ul class="nav nav-pills">
            {#each menuItems as item, i}
                <li class="nav-item"><a href={item.path} class="nav-link" class:active={ i == 0 ? URL == item.path : URL.startsWith(item.path) }>{item.title}</a></li>
            {/each}
        </ul>
        {#if user}
            <p>Добро пожаловать, {user.firstName}</p>
        {:else}
            <button type="button" class="btn btn-primary mb-3" on:click={modal.open}>Войти</button>
        {/if}
    </header>
</div>

<main class="pt-5">
    <slot />
</main>

<Modal bind:this={ modal } hasFooter={ false } title="Авторизация">
    <Form method="POST" action="{API_URL}/auth/login" content="application/json">
        <div class="mb-3">
            <label for="email" class="form-label">Email</label>
            <input type="text" name="email" class="form-control"/>
        </div>
        <div class="mb-3">
            <label for="password" class="form-label">Пароль</label>
            <input type="password" name="password" class="form-control"/>
        </div>
        <button class="btn btn-primary mb-3">Войти</button>
    </Form>
</Modal>
