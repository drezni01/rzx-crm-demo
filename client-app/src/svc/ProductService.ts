import {HoistService, LoadSpec, SelectOption, XH} from '@xh/hoist/core';
import {Product} from '../data/DataTypes';
import {csApiUrl} from './Defaults';

export class ProductService extends HoistService {
    private products: Map<number, Product> = new Map();

    override async initAsync(): Promise<void> {
        await this.loadAsync();
    }

    override async doLoadAsync(loadSpec: LoadSpec): Promise<void> {
        const products = await this.fetchProducts();
        products.forEach(product => {
            this.products.set(product.productId, product);
        });
    }

    get productsList(): Product[] {
        return Array.from(this.products.values());
    }

    getProductById(productId: number): Product {
        return this.products.get(productId);
    }

    get productOptions(): SelectOption[] {
        return this.productsList.map(product => ({
            value: product.productId,
            label: product.name
        }));
    }

    private async fetchProducts(): Promise<Product[]> {
        return XH.fetchJson({
            url: `${csApiUrl}/products`
        });
    }
}
