import { Component, OnInit } from '@angular/core';
import { ProductsApi } from '../services/products-api';
import { ProductDto } from '../api/products-client/models/';

@Component({
  selector: 'app-product-list',
  standalone: false,
  templateUrl: './product-list.html',
  styleUrl: './product-list.css'
})
export class ProductList implements OnInit {

  products: ProductDto[] = []; 
  loading: boolean = true;   
  error: string | null = null; 

  constructor(private productsApi: ProductsApi){
  } 

  ngOnInit(): void {
    this.loadProducts();
  }

  async loadProducts(): Promise<void> {
    this.loading = true;
    this.error = null; 

    try {
      const fetchedProducts = await this.productsApi.getProducts();

      if (fetchedProducts) {
        this.products = fetchedProducts;
      } else {
        this.products = [];
        console.warn('Keine Produkte von der API erhalten.');
      }
    } catch (err) {
      console.error('Fehler beim Laden der Produkte:', err);
      this.error = 'Fehler beim Laden der Produkte. Bitte versuchen Sie es später erneut.';
    } finally {
      this.loading = false;
    }
  }
}
