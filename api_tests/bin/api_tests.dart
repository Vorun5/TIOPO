import 'package:api_tests/api_services.dart';

void main(List<String> arguments) async {
  await ApiServices.getAllProducts();

  // for (var i = 19100; i < 20000; i++) {
  //   final response = await ApiServices.deleteProduct(i);
  //   if (response.item1.status == 1) {
  //     print('delete $i');
  //   }
  //   if (response.item1.status == 0) {
  //     print('not delete $i');
  //   }
  // }
  // final products = await ApiServices.getAllProducts();

  // for (var element in products.item1) {
  //   print(element);
  // }

  // final result = await ApiServices.deleteProduct(18713);

  // print(result.item1);

  // print(result.item2);

  // for (var element in [1, 2, 3, 4]) {
  //   print(element);
  // }

  //    final product = Product(
  //     content: 'fn content 123 <.)][-()>%',
  //     price: 100,
  //     status: 1,
  //     keywords: 'fn keyword',
  //     description: 'fn descriptin',
  //     hit: 1,
  //     categoryId: 1,
  //     oldPrice: 101,
  //   );
  //  final r=  await ApiServices.addProduct(product);

  //   print(r.item1);
  //   print(r.item2.toString());
  //  print(title);
}
