import { Component, OnInit } from '@angular/core';
import { ProductsApi } from '../services/products-api';
import { CategoryDto } from '../api/products-client/models/';

@Component({
  selector: 'app-category-list',
  standalone: false,
  templateUrl: './category-list.html',
  styleUrl: './category-list.css'
})
export class CategoryList implements OnInit {

  categories: CategoryDto[] = []; 
  loading: boolean = true;
  error: string | null = null;

  constructor(private productsApi: ProductsApi) { }

  ngOnInit(): void {
    this.loadCategories(); 
  }

  async loadCategories(): Promise<void> {
    this.loading = true;
    this.error = null;

    try {
      const fetchedCategories = await this.productsApi.getCategories();

      if (fetchedCategories) {
        this.categories = fetchedCategories;
      } else {
        this.categories = [];
        console.warn('Keine Kategorien von der API erhalten.');
      }
    } catch (err) {
      console.error('Fehler beim Laden der Kategorien:', err);
      this.error = 'Fehler beim Laden der Kategorien. Bitte versuchen Sie es später erneut.';
    } finally {
      this.loading = false;
    }
  }
}
