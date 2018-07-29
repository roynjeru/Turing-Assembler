-- Initialization:
{states: Q0,Q1,Q2,Q3,Q4,Q5,Q6,Q7,A,R}
{start: Q0}
{accept: A}
{reject: R}
{alpha: 0,1,#}
{tape-alpha: 0,1,#,x}

-- Main Algorithm:
-- 0:
rwRt Q0 0 x Q1; -- Read, Write, Right, Transition.
rRl Q1 0; -- Read, Right, Loop.
rRl Q1 1;
rRt Q1 # Q3; -- Read, Right, transition.
rRl Q3 x;
rwLt Q3 0 x Q5; -- Read, Write, Left, Transition.

-- 1:
rwRt Q0 1 x Q2;
rRl Q2 0; -- Read, Right, Loop.
rRl Q2 1;
rRt Q2 # Q4; -- Read, Right, transition.
rRl Q4 x;
rwLt Q4 1 x Q5; -- Read, Write, Left, Transition.

-- Find #:
rLl Q5 x; -- Read, Left, Loop.
rLt Q5 # Q6; -- Read, Left, Transition.

-- Reset:
rLl Q6 0;
rLl Q6 1;
rRt Q6 x Q0;

-- Accept:
rRt Q0 # Q7;
rRl Q7 x;
rRt Q7 _ A;