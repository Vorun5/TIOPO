import 'package:api_tests/product.dart';

int _currentTimeInSeconds() {
  var ms = DateTime.now().millisecondsSinceEpoch;

  return (ms / 1000).round();
}

class TestProductData {
  static Product get validProduct {
    final title = _currentTimeInSeconds().toString();

    return Product(
      title: title,
      alias: title,
      content: 'fn content',
      price: 100,
      status: 0,
      keywords: 'fn keyword',
      description: 'fn descriptin',
      hit: 0,
      categoryId: 1,
      oldPrice: 101,
    );
  }

  static final defualtProductStatus = 1;
  static final maxProductCategoryId = 15;
  static final minProductCategoryId = 1;
  static final defualtProductCategoryId = 1;
  static final defualtProductHit = 1;

  static final productsWithIncorrectStatuses = [
    TestProductData.validProduct.copyWith(status: -1),
    TestProductData.validProduct.copyWith(status: 2)
  ];

  static final productsWithIncorrectHits = [
    TestProductData.validProduct.copyWith(hit: -1),
    TestProductData.validProduct.copyWith(hit: 2)
  ];

  static final productsWithInvalidePrice = [
    TestProductData.validProduct.copyWith(price: 0),
    TestProductData.validProduct.copyWith(price: -100),
    TestProductData.validProduct.copyWith(price: -0.1),
    TestProductData.validProduct.copyWith(price: null),
  ];

  static final productsWithInvalideOldPrice = [
    TestProductData.validProduct.copyWith(price: 0),
    TestProductData.validProduct.copyWith(price: -100),
    TestProductData.validProduct.copyWith(price: -0.1),
    TestProductData.validProduct.copyWith(price: null),
  ];

  static final productsWithoutRequiredFields = <String, Product>{
    'title (empty)': TestProductData.validProduct.copyWith(title: ""),
    'title (null)': TestProductData.validProduct.copyWith(title: null),
    'old_price': TestProductData.validProduct.copyWith(oldPrice: null),
    'price': TestProductData.validProduct.copyWith(price: null),
    'category_id': TestProductData.validProduct.copyWith(categoryId: null),
  };

  static final productsWithNegativeCategoryId = [
    TestProductData.validProduct.copyWith(categoryId: -1),
    TestProductData.validProduct.copyWith(categoryId: -12)
  ];

  static final productsWithCategoryIdOutOfRange = [
    TestProductData.validProduct.copyWith(categoryId: minProductCategoryId - 1),
    TestProductData.validProduct.copyWith(categoryId: maxProductCategoryId + 1),
  ];

  static final productsWithValidCategoryId = [
    TestProductData.validProduct.copyWith(categoryId: minProductCategoryId),
    TestProductData.validProduct.copyWith(categoryId: maxProductCategoryId),
  ];

  static final productsWithFieldsOutOfRange = <String, Product>{
    'status can be greater than zero':
        TestProductData.validProduct.copyWith(status: -1),
    'status can be negative': TestProductData.validProduct.copyWith(status: 2),
    'hit can be greater than zero':
        TestProductData.validProduct.copyWith(hit: -1),
    'hit can be negative': TestProductData.validProduct.copyWith(hit: 2),
    'category_Id cannot be greater than 15':
        TestProductData.validProduct.copyWith(categoryId: 16),
    'category_Id cannot be less than 1':
        TestProductData.validProduct.copyWith(categoryId: 0),
  };
}
