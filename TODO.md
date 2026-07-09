**Current phase status**

- Range compiler extension: implemented and tested.
- Intersect restoration: partially implemented (template lookup/render + focused unit diagnostics), not yet fully closed with integration/parity evidence.
- Geopolygon restoration: not implemented yet in this pass.

1. Continue now with geopolygon template lookup/render path and sites_polygon focused tests.
2. Finish intersect closure by adding focused integration evidence and SQL-structure assertions.
3. Investigate and resolve the route config validation runtime blocker so the authoring check can pass end-to-end.