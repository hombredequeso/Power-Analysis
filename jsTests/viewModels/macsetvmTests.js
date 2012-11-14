TestCase("macsetvmTests", {

    test_moduleExists: function () {
	var module = window.HDQ.macsetvm;
	assertNotUndefined(module);
	assertNotNull(module);
    }
});
