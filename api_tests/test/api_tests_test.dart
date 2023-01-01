import 'dart:io';

import 'package:api_tests/answer.dart';
import 'package:api_tests/api_services.dart';
import 'package:api_tests/product.dart';
import 'package:api_tests/test_product_data.dart';
import 'package:test/test.dart';

void main() {
  group('get all porducts', () {
    test('-> you can get all products', () async {
      final response = await ApiServices.getAllProducts();

      expect(
        response.item2,
        HttpStatus.ok,
        reason: 'server response is not ok',
      );
      expect(
        response.item1.isNotEmpty,
        true,
        reason: 'products is empty',
      );
    });
  });
  group('add and delete product', () {
    test('-> valid product added and deleted successfully', () async {
      final product = TestProductData.validProduct;

      final addProductResponse = await ApiServices.addProduct(product);

      expect(
        addProductResponse.item2,
        HttpStatus.ok,
        reason: 'server response is not ok',
      );
      expect(
        addProductResponse.item1.status,
        1,
        reason: 'response status does not match result',
      );

      final getAllProductResponse = await ApiServices.getAllProducts();

      expect(
        getAllProductResponse.item1.contains(
          product.copyWith(id: addProductResponse.item1.id),
        ),
        true,
        reason: 'the added product was not found in the product list',
      );

      final deleteProductResponse =
          await ApiServices.deleteProduct(addProductResponse.item1.id as int);
      final getAllProductResponse2 = await ApiServices.getAllProducts();

      expect(
        getAllProductResponse2.item2,
        HttpStatus.ok,
        reason: 'server response is not ok',
      );
      expect(
        deleteProductResponse.item1.status,
        1,
        reason: 'response status does not match result',
      );
      expect(
        getAllProductResponse2.item1.contains(
          product.copyWith(id: addProductResponse.item1.id),
        ),
        false,
        reason: 'product is not removed',
      );
    });
  });
  group('edit product', () {
    test('-> the product is successfully changed to a valid one', () async {
      final addProductResponse =
          await ApiServices.addProduct(TestProductData.validProduct);

      final product = TestProductData.validProduct
          .copyWith(id: addProductResponse.item1.id);
      final editProductResponse = await ApiServices.editProduct(product);

      final getAllProductResponse = await ApiServices.getAllProducts();

      expect(
        editProductResponse.item1,
        Answer(status: 1),
        reason: 'response status does not match result',
      );
      expect(
        getAllProductResponse.item1.contains(product),
        true,
        reason: 'the edited product is missing',
      );

      await ApiServices.deleteProduct(product.id as int);
    });
  });
  group('checking for required fields', () {
    TestProductData.productsWithoutRequiredFields.forEach((field, product) {
      test('-> when adding $field must be required', () async {
        expect(
          ApiServices.addProduct(product),
          throwsA(isException),
          reason: 'without the $field field, the product should not be added',
        );
      });
    });

    TestProductData.productsWithoutRequiredFields.forEach((field, product) {
      test('-> when editing $field must be required', () async {
        final addProductResponse =
            await ApiServices.addProduct(TestProductData.validProduct);

        expect(
          ApiServices.editProduct(
              product.copyWith(id: addProductResponse.item1.id)),
          throwsA(isException),
          reason:
              'without the $field field, the product should not be edited',
        );

        await ApiServices.deleteProduct(addProductResponse.item1.id as int);
      });
    });
  });
  group('checking products fields for valid values', () {
    test(
      '-> if when adding the status is not 1 or 0, then it is replaced by the default value (${TestProductData.defualtProductStatus})',
      () async {
        for (var product in TestProductData.productsWithIncorrectStatuses) {
          final addProductResponse = await ApiServices.addProduct(product);
          final getAllProductsResponse = await ApiServices.getAllProducts();

          expect(
            getAllProductsResponse.item1.contains(
              product.copyWith(
                id: addProductResponse.item1.id,
                status: TestProductData.defualtProductStatus,
              ),
            ),
            true,
          );

          await ApiServices.deleteProduct(addProductResponse.item1.id as int);
        }
      },
    );
    test(
      '-> if when editing the status is not 1 or 0, then it is replaced by the default value (${TestProductData.defualtProductStatus})',
      () async {
        for (final product in TestProductData.productsWithIncorrectStatuses) {
          final addProductResponse =
              await ApiServices.addProduct(TestProductData.validProduct);
          final id = addProductResponse.item1.id;

          await ApiServices.editProduct(product.copyWith(id: id));
          final getAllProductsResponse = await ApiServices.getAllProducts();

          final editidedProduct =
              getAllProductsResponse.item1.firstWhere((p) => p.id == id);

          expect(
            editidedProduct.equal(
              product.copyWith(
                id: id,
                status: TestProductData.defualtProductStatus,
              ),
            ),
            true,
          );

          await ApiServices.deleteProduct(addProductResponse.item1.id as int);
        }
      },
    );
    test(
      '-> if when adding the hit is not 1 or 0, then it is replaced by the default value (${TestProductData.defualtProductHit})',
      () async {
        for (var product in TestProductData.productsWithIncorrectHits) {
          final addProductResponse = await ApiServices.addProduct(product);
          final getAllProductsResponse = await ApiServices.getAllProducts();

          expect(
            getAllProductsResponse.item1.contains(
              product.copyWith(
                id: addProductResponse.item1.id,
                hit: TestProductData.defualtProductHit,
              ),
            ),
            true,
          );

          await ApiServices.deleteProduct(addProductResponse.item1.id as int);
        }
      },
    );
    test(
      '-> if when editing the hit is not 1 or 0, then it is replaced by the default value (${TestProductData.defualtProductHit})',
      () async {
        for (final product in TestProductData.productsWithIncorrectHits) {
          final addProductResponse =
              await ApiServices.addProduct(TestProductData.validProduct);
          final id = addProductResponse.item1.id;

          await ApiServices.editProduct(product.copyWith(id: id));
          final getAllProductsResponse = await ApiServices.getAllProducts();

          final editidedProduct =
              getAllProductsResponse.item1.firstWhere((p) => p.id == id);

          expect(
            editidedProduct.equal(
              product.copyWith(
                id: id,
                hit: TestProductData.defualtProductHit,
              ),
            ),
            true,
          );

          await ApiServices.deleteProduct(addProductResponse.item1.id as int);
        }
      },
    );
    test(
      '-> when adding category_id must not be negative',
      () async {
        for (var product in TestProductData.productsWithNegativeCategoryId) {
          expect(
            ApiServices.addProduct(product),
            throwsA(isException),
            reason: 'product should not be added',
          );
        }
      },
    );
    test(
      '-> when editing category_id must not be negative',
      () async {
        for (var product in TestProductData.productsWithNegativeCategoryId) {
          final addProductResponse =
              await ApiServices.addProduct(TestProductData.validProduct);
          final id = addProductResponse.item1.id;

          expect(
            ApiServices.editProduct(product.copyWith(id: id)),
            throwsA(isException),
            reason: 'product should not be edited',
          );

          await ApiServices.deleteProduct(id as int);
        }
      },
    );
    test(
      '-> when adding category_id must be in the range of valid values',
      () async {
        for (var product in TestProductData.productsWithCategoryIdOutOfRange) {
          expect(
            ApiServices.addProduct(product),
            throwsA(isException),
            reason: 'product should not be added',
          );
        }
      },
    );
    test(
      '-> when editing category_id must be in the range of valid values',
      () async {
        for (final product
            in TestProductData.productsWithCategoryIdOutOfRange) {
          final addProductResponse =
              await ApiServices.addProduct(TestProductData.validProduct);
          final id = addProductResponse.item1.id;

          expect(
            ApiServices.editProduct(product.copyWith(id: id)),
            throwsA(isException),
            reason: 'product should not be edited',
          );
        }
      },
    );
    test(
      '-> a product with an invalid price should not be added',
      () async {
        for (var product in TestProductData.productsWithInvalidePrice) {
          expect(
            ApiServices.addProduct(product),
            throwsA(isException),
            reason: 'product should not be added',
          );
        }
      },
    );
    test(
      '-> a product with an invalid old price should not be added',
      () async {
        for (var product in TestProductData.productsWithInvalideOldPrice) {
          expect(
            ApiServices.addProduct(product),
            throwsA(isException),
            reason: 'product should not be added',
          );
        }
      },
    );
    test(
      '-> it is impossible to edit the price of the goods to invalid',
      () async {
        for (final product in TestProductData.productsWithInvalidePrice) {
          final addProductResponse =
              await ApiServices.addProduct(TestProductData.validProduct);
          final id = addProductResponse.item1.id;

          expect(
            ApiServices.editProduct(product.copyWith(id: id)),
            throwsA(isException),
            reason: 'product should not be edited',
          );
        }
      },
    );
    test(
      '-> it is impossible to edit the old price of the goods to invalid',
      () async {
        for (final product in TestProductData.productsWithInvalideOldPrice) {
          final addProductResponse =
              await ApiServices.addProduct(TestProductData.validProduct);
          final id = addProductResponse.item1.id;

          expect(
            ApiServices.editProduct(product.copyWith(id: id)),
            throwsA(isException),
            reason: 'product should not be edited',
          );
        }
      },
    );
  });
  group('product alias testing', () {
    test(
      '-> if the product name already exists, the alias must be prefixed with -0',
      () async {
        final product = TestProductData.validProduct;
        await ApiServices.addProduct(product);
        final addProductResponse = await ApiServices.addProduct(product);
        final getAllProductResponse = await ApiServices.getAllProducts();

        expect(
          getAllProductResponse.item1.contains(
            product.copyWith(
              id: addProductResponse.item1.id,
              alias: '${product.alias as String}-0',
            ),
          ),
          true,
          reason: 'the product was not found in the product list',
        );

        await ApiServices.deleteProduct(addProductResponse.item1.id as int);
      },
    );
    test(
      '-> if you edit a product, then the prefix -id must be added to its alias',
      () async {
        final product = TestProductData.validProduct;
        final addProductResponse = await ApiServices.addProduct(product);
        final id = addProductResponse.item1.id;
        await ApiServices.editProduct(product.copyWith(id: id));
        final getAllProductResponse = await ApiServices.getAllProducts();

        expect(
          getAllProductResponse.item1.contains(
            product.copyWith(
              id: addProductResponse.item1.id,
              alias: '${product.alias as String}-$id',
            ),
          ),
          true,
          reason: 'the product was not found in the product list',
        );

        await ApiServices.deleteProduct(addProductResponse.item1.id as int);
      },
    );
  });
}