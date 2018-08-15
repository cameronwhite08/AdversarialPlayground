import keras
from keras.models import Sequential
from keras.layers import Dense, Dropout, Activation
from keras.optimizers import SGD
import numpy as np
import pickle


def load_pickle(filename):
    file = open(filename, 'rb')
    obj = pickle.load(file)
    file.close()
    return obj


def save_data(item, filename):
    output = open(filename, 'wb')
    pickle.dump(item, output)
    output.close()


xs = []
ys = []
all_data = load_pickle('all22936.pkl')
for x, y in all_data:
    t = np.array([d for d in x])
    inx = int()
    for i in range(len(x)):
        if x[i] != y[i]:
            inx = i
    xs.append(t)
    ys.append(inx)

x_train = np.array(xs)
y_train = keras.utils.to_categorical(ys, num_classes=9)

model = Sequential()
# Dense(64) is a fully-connected layer with 64 hidden units.
# in the first layer, you must specify the expected input data shape:
# here, 20-dimensional vectors.
model.add(Dense(9, activation='relu', input_dim=9))
model.add(Dense(9, activation='softmax'))

# sgd = SGD(lr=0.01, decay=1e-6, momentum=0.9, nesterov=True)
model.compile(loss='categorical_crossentropy',
              optimizer='adam',
              metrics=['accuracy'])

model.fit(x_train, y_train,
          epochs=2000,
          batch_size=128)
score = model.evaluate(x_train, y_train)
for layer in model.layers:
    weights = layer.get_weights() # list of numpy arrays
    print(weights)
