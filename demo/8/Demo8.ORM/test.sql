﻿CREATE TABLE `Test`  (
  `Id` int NOT NULL,
  `Name` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;


INSERT INTO `Test` VALUES (1, '工良');
INSERT INTO `Test` VALUES (2, '工良');
INSERT INTO `Test` VALUES (3, '工良');
INSERT INTO `Test` VALUES (4, '工良');