context ("vectors and matrices")

test_that ("calling a function on a matrix", {
    skip_if_not (is.net.installed(), "rDotNet built without .NET CLR server build")

    mat <- cbind(c(4,12,-16.0), c(12,37,-43.0), c(-16,-43,98.0))
    chol <- .cstatic ("MathNet.Numerics.LinearAlgebra.Double.Factorization.DenseCholesky", "Create", mat)

    det <- chol$Get("Determinant")

    expect_equal(36, det)
})
