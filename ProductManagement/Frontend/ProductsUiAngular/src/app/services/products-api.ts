import { Injectable } from '@angular/core';
import { AnonymousAuthenticationProvider } from '@microsoft/kiota-abstractions';
import { FetchRequestAdapter } from '@microsoft/kiota-http-fetchlibrary';

import {
  ProductDto,
  CreateProductCommand,
  UpdateProductCommand,
  DeleteProductCommand,
} from '../api/products-client/models/';

import {
  CategoryDto,
  CreateCategoryCommand,
  UpdateCategoryCommand,
  DeleteCategoryCommand
} from '../api/products-client/models/';

import { Guid } from '@microsoft/kiota-abstractions';
import { ProductsClient, createProductsClient } from '../api/products-client/productsClient';


@Injectable({
  providedIn: 'root'
})
export class ProductsApi {
  private productsClient: ProductsClient;

  constructor() {
    const authProvider = new AnonymousAuthenticationProvider();
    const adapter = new FetchRequestAdapter(authProvider);
    adapter.baseUrl = 'https://localhost:7294'; // https://localhost:7294, http://localhost:5044 
    this.productsClient = createProductsClient(adapter);
  }

  /** Products **/

  /**
   * Ruft alle Produkte ab.
   * Deine `ProductsRequestBuilderGetQueryParameters` sind optional, können aber übergeben werden
   * für Paginierung, Sortierung, Suche etc.
   */
  getProducts(
    options?: {
      ascending?: boolean;
      orderBy?: string;
      pageNumber?: number;
      pageSize?: number;
      searchTerm?: string;
    }
  ): Promise<ProductDto[] | undefined> {
    const requestConfig = options ? { queryParameters: options } : undefined;
    return this.productsClient.products.get(requestConfig);
  }

  /**
   * Ruft ein einzelnes Produkt anhand seiner ID ab.
   * Die ID ist vom Typ Guid.
   */
  getProductById(id: Guid): Promise<ProductDto | undefined> {
    return this.productsClient.products.byId(id).get();
  }

  /**
   * Erstellt ein neues Produkt.
   * Erfordert ein `CreateProductCommand` Objekt.
   * Gibt eine GUID (die ID des neu erstellten Produkts) zurück.
   */
  createProduct(command: CreateProductCommand): Promise<Guid | undefined> {
    return this.productsClient.products.post(command);
  }

  /**
   * Aktualisiert ein bestehendes Produkt.
   * Erfordert ein `UpdateProductCommand` Objekt.
   * Gibt `ArrayBuffer` zurück, was oft auf einen leeren Body (204 No Content) hinweist.
   */
  updateProduct(command: UpdateProductCommand): Promise<ArrayBuffer | undefined> {
    return this.productsClient.products.put(command);
  }

  /**
   * Löscht ein Produkt.
   * Erfordert ein `DeleteProductCommand` Objekt.
   * Gibt nichts zurück (void) bei Erfolg.
   */
  deleteProduct(command: DeleteProductCommand): Promise<void> {
    return this.productsClient.products.delete(command);
  }

  /** Categories **/

  /**
   * Ruft alle Kategorien ab.
   */
  getCategories(): Promise<CategoryDto[] | undefined> {
    return this.productsClient.categories.get();
  }
}
