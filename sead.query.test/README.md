



## Glossary

- Fake - A generic term for stubs stub or a mocks.
  - Stub - A concrete replacement for an dependency (represents state).
  - Mock - A fake that is a stub that also can verify behavioural aspects of the fake e.q. test that a method has been called.


"One of the principles of a unit test is that it **must have full control** of the system under test."

## Best practices

1. Write unit tests that are *fast*, *isolated* and *repeatable*.
1. Write tests that are simple and fast to code - otherwise consider using a more "testable" design!
1. Write minimal tests using simplest possible input data.

1. Use "Method name" + "Scenario being tested" + "Expected behaviour" naming convention.
1. Use the "Arrange, Act, Assert" test arrangement pattern aka AAA.
1. Use helper methods/classes for setup and teardown
1. Use stubs to isolate the subject under test (SUT)

1. Avoid non-null values only if not required
1. Avoid logic i tests
1. Avoid multiple asserts (e.g. use different input parameters to catch edge cases)


1. Only test public methods - don't test your private methods!

Source: https://docs.microsoft.com/en-us/dotnet/core/testing/unit-testing-best-practices

[MSDN ](https://docs.microsoft.com/en-us/visualstudio/test/using-stubs-to-isolate-parts-of-your-application-from-each-other-for-unit-testing)
[…] *your application has to be designed so that the different components are not dependent on each other, but only dependent on interface definitions.
Instead of being coupled at compile time, components are connected at run time. This pattern helps to make software that is robust and easy to
update, because changes tend not to propagate across component boundaries.*

