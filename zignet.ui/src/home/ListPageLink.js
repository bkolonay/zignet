import React, { Component } from 'react';

class ListPageLink extends Component {

  render() {
    return (
      <a href={`/${this.props.suiteId}`}>Total: {this.props.totalTests}</a>
    );
  }
}

export default ListPageLink;
