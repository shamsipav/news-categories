<script lang="ts">
    import { Modal, Form } from '$components'
    import { API_URL } from '$lib/consts'
    import type { ICategory, IUser, ModalComponent } from '../../lib/types'

    export let data: any

    let user: IUser = data.user
    
    let categories: ICategory[] = data.categories

    let modal: ModalComponent = null

    const showNewCategory = (event: CustomEvent<{ message: string, category: ICategory }>) => {
        const newCategory = event.detail.category
        categories = [ ...categories, newCategory ]
        setTimeout(modal.close, 500)
    }
</script>

<div class="container">
    <h2 class="mb-4">Категории</h2>
    {#if user}
        <button type="button" class="btn btn-primary mb-3" on:click={modal.open}>Добавить категорию</button>
    {:else}
        <p class="lead">Добавлять категории могут только авторизированные пользователи</p>
    {/if}
    {#if categories}
        <div class="categories-grid">
            {#each categories as category}
                <div class="card">
                    <div class="card-body">
                        <div class="d-flex align-items-cente justify-content-between mb-2">
                            <h5 class="card-title mb-0 me-2">{category.name}</h5>
                            {#if category.news.length}
                                <div>
                                    <span class="badge bg-info">{ category.news.length }</span>
                                </div>
                            {/if}
                        </div>
                        <a href="/categories/{category.id}" class="btn btn-light btn-sm">Открыть</a>
                    </div>
                </div>
            {/each}
        </div>
    {:else}
        <p>Загрузка категорий...</p>
    {/if}
</div>
<Modal bind:this={ modal } hasFooter={ false } title="Добавление категории">
    <Form method="POST" action="{API_URL}/category" reset={false} on:success={showNewCategory}>
        <div class="mb-3">
            <label for="name" class="form-label">Название</label>
            <input type="text" name="name" class="form-control" id="name" required />
        </div>
        <button class="btn btn-primary mb-3 btn-sm">Добавить</button>
    </Form>
</Modal>
